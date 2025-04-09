using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class KnockbackUtil
{
    public static void ApplyKnockback(GameObject target, Vector2 fromPosition, float force)
    {
        if (target == null) return;

        Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
        if (rb == null || rb.bodyType != RigidbodyType2D.Dynamic) return;

        Vector2 dir = ((Vector2)target.transform.position - fromPosition).normalized;
        rb.AddForce(dir * force, ForceMode2D.Impulse);
    }
}
