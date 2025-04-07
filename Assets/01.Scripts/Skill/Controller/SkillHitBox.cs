using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillHitBox : MonoBehaviour
{
    public int damage;
    public float duration = 0.3f;

    private void Start()
    {
        Destroy(gameObject, duration); // 일정 시간 후 자동 제거
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            //other.GetComponent<Enemy>()?.TakeDamage(damage);
        }
    }
}
