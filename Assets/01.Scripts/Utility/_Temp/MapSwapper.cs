using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.Serialization;

public class Session
{
    public NavSurface Surface;
    public TilemapRenderer Renderer;
    public Transform MudTransform;
}

public class MapSwapper: MonoBehaviour
{
    public MapSwapper Instance { get; private set; }
    public Transform mudHandPool;
    
    private readonly List<NavSurface> sessions = new();
    private int currSession = 0;
    
    [FormerlySerializedAs("currNavSurface")] public NavSurface currSurface;

    private float currTime;
    public float duration = 5f;
    public float fadeDuration = 1f;
    
    // mud 관련 코드
    public EnemyController mudEye;
    public EnemyController mudHand;
    private readonly List<EnemyController> mudHands = new();

    private void Awake()
    {
        if(!Instance) Instance = this;
        
        foreach (Transform child in transform)
        {
            var nav = child.GetComponent<NavSurface>();
            if (nav != null)
            {
                sessions.Add(nav);
                child.gameObject.SetActive(false);
            }
        }

        if (sessions.Count > 0)
        {
            sessions[0].gameObject.SetActive(true);
        }
    }
    
    // private void Update()
    // {
    //     currTime += Time.deltaTime;
    //     if (currTime >= duration)
    //     {
    //         currTime = 0;
    //         SwapMapAsync().Forget();
    //     }
    // }

    // ReSharper disable Unity.PerformanceAnalysis
    public async UniTaskVoid SwapMapAsync()
    {
        if (sessions.Count < 2) return;

        var prevSurface = sessions[currSession];
        var nextSession = (currSession + 1) % sessions.Count;
        var nextSurface = sessions[nextSession];

        // 비용 문제 - 객체로 관리하기
        var currRenderer = prevSurface.GetComponent<TilemapRenderer>();
        var nextRenderer = nextSurface.GetComponent<TilemapRenderer>();

        if (!currRenderer || !nextRenderer)
        {
            Debug.LogWarning("TilemapRenderer가 누락되었습니다.");
            return;
        }

        // 인스턴스 머티리얼로 교체
        currRenderer.material = new Material(currRenderer.material);
        nextRenderer.material = new Material(nextRenderer.material);

        GameManager.Instance.cameraShake.Shake(0.2f, 1f, 1f);
        
        // 페이드 아웃
        await currRenderer.material.DOFade(0f, fadeDuration).SetEase(Ease.InOutQuad).AsyncWaitForCompletion();
        mudHands.ForEach(currMudHand =>
        {
            currMudHand.gameObject.SetActive(false);
            currMudHand.Board.IsSpawned = false;
            currMudHand.Machine.Notify();
        });

        prevSurface.gameObject.SetActive(false);
        nextSurface.gameObject.SetActive(true);

        // 다음 타일맵 초기화
        var nextMat = nextRenderer.material;
        var color = nextMat.color;
        color.a = 0f;
        nextMat.color = color;

        // 페이드 인
        await nextMat.DOFade(1f, fadeDuration).SetEase(Ease.InOutQuad).AsyncWaitForCompletion();
        
        currSurface = nextSurface;
        // 순차 랜덤 생성
        SoundManager.Instance.Playsfx("MudHand_Appear");
        
        var bossIndex = Random.Range(0, currSurface.platforms.Count);
        for (var i = 0; i < currSurface.platforms.Count; i++)
        {
            if (i == bossIndex)
            {
               mudEye.transform.position = currSurface.platforms[i].centerCell.WorldPos + Vector2.up * 2f;
               continue;
            }
               
            var platform = currSurface.platforms[i];
            Cell[] cells = { platform.startCell, platform.centerCell, platform.endCell };
            var newPos = cells[Random.Range(0, 3)].WorldPos;
            
            var currMud = mudHands.Find(mudhand => !mudhand.gameObject.activeSelf);
            if (!currMud)
            {
                currMud = Instantiate(mudHand, newPos, Quaternion.identity);
                currMud.transform.SetParent(mudHandPool);
                mudHands.Add(currMud);
            }
            currMud.transform.position = newPos;
            currMud.gameObject.SetActive(true);
            currSession = nextSession;
        }
    }
}
