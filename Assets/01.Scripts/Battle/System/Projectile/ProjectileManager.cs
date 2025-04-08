using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : Singleton<ProjectileManager>
{
    public GameObject meleeProjectile;
    public List<GameObject> projectileList;
    public List<(int index, GameObject instance)> currProjectiles = new();

    // ReSharper disable Unity.PerformanceAnalysis
    public void CreateMeleeProjectile(Transform parent, float power, Vector2? startPos = null)
    {
        var instance = parent.GetComponentInChildren<HitBox>(true)?.gameObject; // 찾는 방법 필요
        
        if (!instance)
        {
            instance = Instantiate(meleeProjectile, parent);
            instance.transform.SetParent(parent);
            instance.transform.localPosition = startPos ?? Vector2.right;
        }
        
        instance.SetActive(true);
        
        HitBox hitBox = instance.GetComponent<HitBox>();
        hitBox.SetDamage(power);
        hitBox.SetOwner(parent);
    }

    // 비용 문제에 고민해보기
    // ReSharper disable Unity.PerformanceAnalysis
    public void DestroyMeleeProjectile(Transform parent)
    {
        var instance = parent.GetComponentInChildren<HitBox>(true)?.gameObject;
        if(instance) instance.SetActive(false);
    }
    
    // 사이즈 포함
    // ReSharper disable Unity.PerformanceAnalysis
    public void CreateProjectile(Transform parent, float power, Vector2? startPos = null, int index = 0) // melee attack인 경우 우연히 두번 켜지는 현상 방지 필요
    {
        var instance = currProjectiles.Find(projectile => projectile.index == index && !projectile.instance.activeSelf).instance;
        if (!instance)
        {
            instance  = Instantiate(projectileList[index], Vector2.zero, Quaternion.identity, parent);
            currProjectiles.Add((index, instance));
        }
        else
        {
            instance.SetActive(true);
        }

        HitBox hitBox = instance.GetComponent<HitBox>();
        hitBox.SetDamage(power);
        hitBox.SetOwner(parent);
        
        instance.transform.localRotation = Quaternion.Euler(0, 0, -90); // 방향 계산 필요 -90 이 오른쪽
        instance.transform.localPosition = startPos ?? instance.transform.right;
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
    
}
