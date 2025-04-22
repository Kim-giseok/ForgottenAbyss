using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

// direction to degree 같은 것이 필요 할 듯
// 빌더 패턴으로 관리해보면 어떨까? 아무튼 조합의 형태를 띄어야 함
public class BoltsPool : MonoBehaviour // 단위 미사일
{
    public static BoltsPool Instance { get; private set; }
    
    public GameObject meleeBolt;
    public GameObject rangeBolt;
    public GameObject summon;

    public List<GameObject> projectileList; // sprite만 바뀌고 속성이 자유자제라면?
    

    public List<GameObject> rangeBoltList { get; private set; } = new();
    public List<(int index, GameObject instance)> currProjectiles = new(); // notice : HitBox를 가지고 있는 편이 비용 감소
    public List<(GameObject owner, HitBox hitBox)> currMeleeProjectiles = new(); // 만약 여기서 등록하는 경우, 몬스터가 죽으면 함께 제거 필요

    private void Awake()
    {
        if (Instance) return;
        
        Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        SceneManager.activeSceneChanged += (Scene oldScene, Scene newScene) =>
        {
            currMeleeProjectiles.Clear();
        };
    }
    
    public static float GetDegreeByDirection(Vector2 direction)
    {
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    // do: 사이즈의 조절, 모양의 변경 등의 관리 필요
    public void CreateMelee(Transform parent, float power, Vector2? startPos = null, Vector2? size = null)
    {
        var instance = parent.GetComponentInChildren<HitBox>(true)?.gameObject; // 찾는 방법 필요
        if (!instance) { instance = Instantiate(meleeBolt, parent); }
        
        // if (startPos == null) instance.transform.localPosition = transform.right;
        instance.transform.localPosition = startPos ?? transform.right;
        
        instance.transform.localScale = Vector3.one; // 사이즈 지정이 따로 있다면 적용
        
        HitBox hitBox = instance.GetComponent<HitBox>();
        hitBox.SetDamage(power);
        
        // bug: 충돌이 우선 발생하여 인식하지 못하는 현상 발생
        hitBox.SetOwner(parent);

        instance.transform.localScale = size ?? Vector3.one;
        
        instance.SetActive(true);

    }

    // 비용 문제에 고민해보기
    // ReSharper disable Unity.PerformanceAnalysis
    public void DestroyMelee(Transform parent)
    {
        var instance = parent.GetComponentInChildren<HitBox>(true)?.gameObject;
        if(instance) instance.SetActive(false);
    }
    
    // 사이즈 포함
    // 반사 또는 유도
    // 빌더 패턴으로 조립 필요
    // 모든 프로젝타일을 재사용하는 방향으로 변경
    // ReSharper disable Unity.PerformanceAnalysis
    public void Create(Transform parent, float power, Vector2 startPos = default,  int index = 0, float degree = 0, bool isLocalPosition = true) // melee attack인 경우 우연히 두번 켜지는 현상 방지 필요
    {
        // 비활성화된 프로젝타일 찾기
        var instance = currProjectiles.Find(projectile => !projectile.instance.activeSelf).instance;
        if (!instance)
        {
            // 발사체는 프로젝타일 매니저에서 관리 - 생성되고 바로 발사되선 안됨
            instance  = Instantiate(projectileList[index], Vector2.zero, Quaternion.identity, transform); 
            currProjectiles.Add((index, instance));
        }

        HitBox hitBox = instance.GetComponent<HitBox>(); // notice: HitBox 자체를 저장하도록 변경 필요

        if (hitBox)
        {
            hitBox.SetDamage(power);
            hitBox.SetOwner(parent);
        }
        
        
        instance.transform.rotation = Quaternion.Euler(0, 0, degree);

        // instance.gameObject.layer = parent.gameObject.layer; // 직접적으로 레이어 할당은 하지 않음 - 플레이어와 애너미간 접촉 관련 문제로 인해
        instance.transform.position = parent.position;
        
        instance.SetActive(true);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public Bolt Create(Transform parent, Bolts.Type boltType)
    {
        var instance = rangeBoltList.Find(bolt => !bolt.activeSelf);
        
        if (!instance)
        {
            instance = Instantiate(rangeBolt, Vector2.zero, Quaternion.identity, transform);
            rangeBoltList.Add(instance);
        }

        Bolt bolt = instance.GetComponent<Bolt>();
        HitBox hitBox = bolt.hitBox;
        hitBox.SetOwner(parent);
        
        // 플레이어 피봇 문제로 위치 조정 필요
        instance.transform.position = parent.position + (Vector3.up * 0.5f);
        
        bolt.direction = parent.transform.right;
        bolt.machine.Define(Bolts.Get(boltType));
        
        return bolt;
    }
    
    public void Destroy(GameObject instance)
    {
        var selectedProjectile = currProjectiles.Find(projectile => projectile.instance == instance).instance;
        instance.SetActive(false);
    }
    
    // 팩토리 패턴과 빌더 패턴을 합쳐서 사용하고 싶다. - summon도 재사용 개념이 필요한지 확인해보기
    // 각도로 넣어주기 
    public void CreateSummon(Transform parent, SummonSkillManager.Skill skill, bool isAttached = false)
    {
        // notice: 플레이어 위치로 인한 보정 필요
        GameObject instance = Instantiate(summon, new Vector2(parent.transform.position.x, parent.transform.position.y + 0.8f), Quaternion.identity);
        
        instance.gameObject.layer = parent.gameObject.layer;
        
        SummonController summonController = instance.GetComponent<SummonController>();
        summonController.SetCaster(parent, isAttached);
        summonController.ExecuteSkill(skill);
    }
}
