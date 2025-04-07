using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : Singleton<ProjectileManager>
{
    public List<GameObject> projectileList;
    public List<(int index, GameObject instance)> currProjectiles = new();
    
    // 사이즈 포함
    // ReSharper disable Unity.PerformanceAnalysis
    public void CreateProjectile(Transform parent, float power, Vector2? currPos = null, int index = 0)
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
        hitBox.SetParent(parent);
        
        instance.transform.localRotation = Quaternion.Euler(0, parent.eulerAngles.y, 0);
        instance.transform.localPosition = currPos ?? instance.transform.right;
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
