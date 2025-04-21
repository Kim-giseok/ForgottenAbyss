using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSpawnManager : Singleton<MapSpawnManager>
{
    [SerializeField] private Map[] maps;
    [SerializeField] CinemachineConfiner2D confiner2D;
    int mapIdx = 0;
    public Map SpawnedMap { get; private set; }
    public Sprite[] backgroundSprites;
    public BackGround backgroundprefeb;
    public BackGround[] backgrounds;

    private void Awake()
    {
        _instance = this;
        mapIdx = 0;
    }

    private void Start()
    {
        CinemachineVirtualCamera virtualCamera = confiner2D.GetComponent<CinemachineVirtualCamera>();
        virtualCamera.Follow = GameManager.Instance.player.transform;

        SpawnBackground();
        SpawnNextMap();
    }

    private void LateUpdate()
    {
        ParrallexScrollBackground();
    }

    public void SpawnNextMap()
    {
        if (mapIdx >= maps.Length)
        {
            SceneManager.LoadScene(1); //이후 마을 신으로 수정
            return;
        }

        if (SpawnedMap != null)
            Destroy(SpawnedMap.gameObject);

        SpawnedMap = Instantiate(maps[mapIdx++]);
        SpawnedMap.MapStart();
        confiner2D.m_BoundingShape2D = SpawnedMap.CameraCollider;
    }

    void SpawnBackground()
    {
        backgrounds = new BackGround[backgroundSprites.Length];
        for (int i = 0; i < backgroundSprites.Length; i++)
        {
            backgrounds[i] = Instantiate(backgroundprefeb, Vector3.zero, Quaternion.identity);
            backgrounds[i].sprite = backgroundSprites[i];
            backgrounds[i].sortingOrder = -100 + i;
        }
    }

    void ParrallexScrollBackground()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float posX = (SpawnedMap.transform.position - Camera.main.transform.position).x * i / backgrounds.Length;
            Vector3 newPosition = Camera.main.transform.position - Vector3.left * posX;
            backgrounds[i].position = new Vector3(newPosition.x, newPosition.y);
        }
    }
}
