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
    
    public GameObject Melee;
    public GameObject Bolt;
    public GameObject Summon;

    private List<GameObject> currBolts { get; set; } = new();
    private List<(Transform parent, HitBox hitBox)> currMelees = new(); // 만약 여기서 등록하는 경우, 몬스터가 죽으면 함께 제거 필요
    private List<GameObject> currSummons = new();
    
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
            currMelees.Clear();
        };
    }
    
    public static float GetDegree(Vector2 direction)
    {
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    // do: 사이즈의 조절, 모양의 변경 등의 관리 필요
    public HitBox CreateMelee(Transform parent, float power, Vector2? startPos = null, Vector2? size = null)
    {
        var currMelee = currMelees.Find(melee => melee.parent == parent);
        if (!currMelee.hitBox)
        {
            GameObject instance = Instantiate(Melee, parent);
            currMelee = (parent, instance.GetComponent<HitBox>());
            currMelees.Add(currMelee);
        }
        
        HitBox hitBox = currMelee.hitBox;
        
        hitBox.transform.localPosition = startPos ?? transform.right;
        hitBox.transform.localScale = size ?? Vector3.one;
        
        // bug: 충돌이 우선 발생하여 인식하지 못하는 현상 발생
        hitBox.SetDamage(power);
        hitBox.SetOwner(parent);
        
        hitBox.gameObject.SetActive(true);

        return hitBox;
    }

    public void DisableMelee(Transform parent)
    {
        HitBox hitBox = currMelees.Find(melee => melee.parent == parent).hitBox;
        if(hitBox) hitBox.gameObject.SetActive(false);
    }

    public void DestroyMelee(Transform parent)
    {
        var selectedMelee = currMelees.Find(melee => melee.parent == parent);
        if(selectedMelee.hitBox) currMelees.Remove(selectedMelee);
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    public Bolt Create(Transform parent, Bolts.Type boltType)
    {
        var instance = currBolts.Find(bolt => !bolt.activeSelf);
        Debug.Log(instance);
        
        if (!instance)
        {
            instance = Instantiate(this.Bolt, Vector2.zero, Quaternion.identity, transform);
            currBolts.Add(instance);
        }

        Bolt bolt = instance.GetComponent<Bolt>();
        HitBox hitBox = bolt.hitBox;
        hitBox.SetOwner(parent);
        
        // 플레이어 피봇 문제로 위치 조정 필요
        instance.transform.position = parent.position + (Vector3.up * 0.5f);
        
        bolt.SetDirection(parent.transform.right);
        bolt.machine.Define(Bolts.Get(boltType));
        
        return bolt;
    }
    
    public void Disable(GameObject instance)
    {
        instance.SetActive(false);
    }
    
    // 팩토리 패턴과 빌더 패턴을 합쳐서 사용하고 싶다. - summon도 재사용 개념이 필요한지 확인해보기
    // 각도로 넣어주기 
    public SummonController CreateSummon(Transform parent, SummonSkillManager.Skill skill, bool isAttached = false)
    {
        // notice: 플레이어 위치로 인한 보정 필요
        GameObject instance = Instantiate(Summon, new Vector2(parent.transform.position.x, parent.transform.position.y + 0.8f), Quaternion.identity); 
        instance.gameObject.layer = parent.gameObject.layer;
        
        SummonController summonController = instance.GetComponent<SummonController>();
        summonController.SetCaster(parent, isAttached);
        summonController.ExecuteSkill(skill);

        return summonController;
    }
}
