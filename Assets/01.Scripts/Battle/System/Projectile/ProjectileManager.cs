using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// direction to degree 같은 것이 필요 할 듯
// 빌더 패턴으로 관리해보면 어떨까? 아무튼 조합의 형태를 띄어야 함
public class ProjectileManager : Singleton<ProjectileManager> // 단위 미사일
{
    public GameObject meleeProjectile;
    public List<GameObject> projectileList;
    public List<EnemyController> enemyProjectileList;
    
    public List<(int index, GameObject instance)> currProjectiles = new(); // notice : HitBox를 가지고 있는 편이 비용 감소
    public List<(GameObject owner, HitBox hitBox)> currMeleeProjectiles = new();
    
    public float GetDegreeByDirection(Vector2 direction)
    {
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    // do: 사이즈의 조절, 모양의 변경 등의 관리 필요
    public void CreateMeleeProjectile(Transform parent, float power, Vector2? startPos = null, Vector2? size = null)
    {
        var instance = parent.GetComponentInChildren<HitBox>(true)?.gameObject; // 찾는 방법 필요
        
        if (!instance)
        {
            instance = Instantiate(meleeProjectile, parent);
            instance.transform.SetParent(parent);
        }
        
        // if (startPos == null) instance.transform.localPosition = transform.right;
        instance.transform.localPosition = startPos ?? transform.right;
        
        instance.transform.localScale = Vector3.one; // 사이즈 지정이 따로 있다면 적용
        
        HitBox hitBox = instance.GetComponent<HitBox>();
        hitBox.SetDamage(power);
        hitBox.SetOwner(parent);

        instance.transform.localScale = size ?? Vector3.one;
        
        instance.SetActive(true);

    }

    // 비용 문제에 고민해보기
    // ReSharper disable Unity.PerformanceAnalysis
    public void DestroyMeleeProjectile(Transform parent)
    {
        var instance = parent.GetComponentInChildren<HitBox>(true)?.gameObject;
        if(instance) instance.SetActive(false);
    }
    
    // 사이즈 포함
    // 반사 또는 유도
    // ReSharper disable Unity.PerformanceAnalysis
    public void CreateProjectile(Transform parent, float power, ProjectileAttr[] attrs, Vector2 startPos = default,  int index = 0, float degree = 0) // melee attack인 경우 우연히 두번 켜지는 현상 방지 필요
    {
        var instance = currProjectiles.Find(projectile => projectile.index == index && !projectile.instance.activeSelf).instance;
        if (!instance)
        {
            instance  = Instantiate(projectileList[index], Vector2.zero, Quaternion.identity, transform); // 발사체는 프로젝타일 매니저에서 관리
            currProjectiles.Add((index, instance));
        }
        else
        {
            instance.SetActive(true);
        }

        HitBox hitBox = instance.GetComponent<HitBox>(); // notice: HitBox 자체를 저장하도록 변경 필요
        hitBox.SetDamage(power);
        hitBox.SetOwner(parent);
        
        Projectile projectile = instance.GetComponent<Projectile>(); // notice: 프로젝타일도 매번 파악하는 현상 발생
        // projectile.AddAttribute(new StraightAttr());
        projectile.AddAttribute(attrs);
        
        
        instance.transform.localRotation = Quaternion.Euler(0, 0, degree - 90); // 화살이 현재 위를 보고 있는 상황이라 방향 계산 필요 -90 이 오른쪽
        instance.transform.localPosition = new Vector2(parent.position.x + startPos.x, parent.position.y + startPos.y);
    }

    public void Repeat()
    {
        
    }

    public void DestroyProjectile(Transform transform)
    {
        var selectedProjectile = currProjectiles.Find(projectile => projectile.instance.transform.parent == transform).instance;
        if(selectedProjectile) selectedProjectile.gameObject.SetActive(false);
    }
    
    public void DestroyProjectile(GameObject instance)
    {
        instance.SetActive(false);
    }

    public void CreateEnemyProjectile(Transform parent, string skillNodeName)
    {
        var currProjectile = enemyProjectileList[0];
        currProjectile.isAwake = false;
        currProjectile.SkillNodeName = skillNodeName;
        Instantiate(enemyProjectileList[0].gameObject, parent.transform.position, Quaternion.identity);
    }
}
