using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePlayer : MonoBehaviour
{
    private Transform player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ProjectileManager.Instance.CreateProjectile(transform, 10);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            ProjectileManager.Instance.DestroyProjectile(transform);
        }
    }
    
    private void FixedUpdate()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        player.position = new Vector3(mousePosition.x, mousePosition.y, 0);
    }
}
