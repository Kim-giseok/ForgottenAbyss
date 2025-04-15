using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaberPush : LaberBase
{
    [SerializeField] Collider2D pushCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.transform.position.y - transform.position.y < pushCollider.offset.y * 2 * transform.localScale.y) return;

        SwitchMachine();
    }
}
