using System;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamagable
{
    [Header("Resource")]
    public float health;
    public float attack;

    private BTMachine btMachine;
    
    private Transform pivot;
    public WeaponSO currWeapon;

    public void Awake()
    {
        btMachine = new(this);
        
 
    }

    private void Start()
    {
        pivot = transform.Find("Pivot");
        if (!pivot) { pivot = new GameObject("Pivot").transform; pivot.parent = transform; }

        GameObject weapon = new("Weapon");
        weapon.AddComponent<SpriteRenderer>().sprite = currWeapon.image;
        weapon.transform.SetParent(pivot);
    }

    public void Flip(bool isFlip)
    {
        transform.rotation = Quaternion.Euler(0, isFlip ? 0 : 180, 0);
    }
    
    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    private void FixedUpdate()
    {
        btMachine.Run();
    }
}
