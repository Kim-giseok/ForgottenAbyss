using System;
using UnityEngine;

public class VFXprojectile: MonoBehaviour
{
    private float currentTime = 0f;
    public float duration;
    private Animator animator;
    

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        currentTime = 0f;
    }

    private void Start()
    {
        animator.Play("Heal");
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= 1f)
        {
            ProjectileManager.Instance.DestroyProjectile(gameObject);
        }
    }
}