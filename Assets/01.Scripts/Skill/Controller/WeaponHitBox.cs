using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitBox : MonoBehaviour
{
    private bool canHit = false;
    public GameObject hitbox;

    public void Init(GameObject hitboxObj)
    {
        this.hitbox = hitboxObj;
        hitbox.SetActive(false);
    }

    public void EnableHitbox()
    {
        hitbox.SetActive(true);
        canHit = true;
        Debug.Log("Hitbox enabled!");
    }

    public void DisableHitbox()
    {
        hitbox.SetActive(false);
        canHit = false;
        Debug.Log("Hitbox disabled!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!canHit) return;

        if (other.CompareTag("Enemy"))
        {
            Debug.Log("hit!");
        }
    }
}
