using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : Singleton<ProjectileManager>
{
    public List<GameObject> projectileList;
    public List<(int index, GameObject instance)> currProjectiles = new();
    
    public GameObject CreateProjectile(Transform currPos, int index)
    {
        var selectedProjectile = currProjectiles.Find(projectile => projectile.index == index && !projectile.instance.activeSelf);
        if (!selectedProjectile.instance)
        {
            GameObject newProjectile  = Instantiate(projectileList[index], currPos.position, Quaternion.identity, transform);
            currProjectiles.Add((index, newProjectile));
            return newProjectile;
        }
        
        selectedProjectile.instance.SetActive(true);
        return selectedProjectile.instance;
    }

    public void DestroyProjectile(GameObject instance)
    {
        instance.SetActive(false);
    }
}
