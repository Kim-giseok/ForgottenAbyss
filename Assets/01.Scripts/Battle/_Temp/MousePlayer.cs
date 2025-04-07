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
            ProjectileManager.Instance.CreateProjectile(transform.position + transform.right, 0);
        }
    }
    
    private void FixedUpdate()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        player.position = new Vector3(mousePosition.x, mousePosition.y, 0);
    }
}
