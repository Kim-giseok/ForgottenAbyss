using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaberPush : LaberBase
{
    [SerializeField] Collider2D pushCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector2 direct = collision.transform.position - transform.position;
        float projectionLength = Vector2.Dot(direct, transform.up);

        if (projectionLength < pushCollider.offset.y * transform.localScale.y) return;

        SwitchMachine();
    }
}
