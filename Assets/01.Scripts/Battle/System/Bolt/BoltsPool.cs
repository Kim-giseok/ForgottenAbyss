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

    private List<Bolt> currBolts { get; set; } = new();
    private List<(Transform parent, HitBox hitBox)> currMelees = new(); // 만약 여기서 등록하는 경우, 몬스터가 죽으면 함께 제거 필요
    private List<SummonController> currSummons = new();
    
    [Serializable] public class SpriteInfo { public string name; public Sprite sprite; }
    public List<SpriteInfo> sprites;
    
    public enum TrailType { Base, Laser }
    [Serializable] public class TrailAnimCurve { [FormerlySerializedAs("name")] public TrailType type; public AnimationCurve curve; }
    public List<TrailAnimCurve> trailAnimCurves;
    
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

    public Sprite GetSprite(string spriteName)
    {
        return sprites.Find(sprite => sprite.name == spriteName).sprite;
    }
    
    public static float GetDegree(Vector2 direction)
    {
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    // HitBox 자체 전달로 빌더 패턴 적용
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
        Bolt bolt = currBolts.Find(bolt => !bolt.gameObject.activeSelf);
        
        if (!bolt)
        {
            GameObject instance = Instantiate(Bolt, Vector2.zero, Quaternion.identity, transform);
            bolt = instance.GetComponent<Bolt>();
            currBolts.Add(bolt);
        }

        HitBox hitBox = bolt.hitBox;
        hitBox.SetOwner(parent);
        
        // 플레이어 피봇 문제로 위치 조정 필요
        // bolt.transform.position = parent.position + (Vector3.up * 0.5f);
        bolt.transform.position = parent.position;
        
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
    // ReSharper disable Unity.PerformanceAnalysis
    public SummonController CreateSummon(Transform parent, SummonSkillManager.Skill skill, bool isAttached = false)
    {
        // SummonController currSummon = currSummons.Find(summon => !summon.gameObject.activeSelf);
        // if (!currSummon)
        // {
            GameObject instance = Instantiate(Summon, new Vector2(parent.transform.position.x, parent.transform.position.y + 0.8f), Quaternion.identity);
            SummonController currSummon = instance.GetComponent<SummonController>();
            // currSummons.Add(currSummon);
        // }
        
        // notice: 플레이어 위치로 인한 보정 필요
        currSummon.gameObject.layer = parent.gameObject.layer;
        
        currSummon.SetCaster(parent, isAttached);
        currSummon.ExecuteSkill(skill);

        return currSummon;
    }
}
