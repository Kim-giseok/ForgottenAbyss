using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWarp : MonoBehaviour
{
    [Header("Ignore position")]
    [SerializeField] bool x;
    [SerializeField] bool y;

    public void WarpPlayerToThis()
    {
        Player player = GameManager.Instance.player;

        player.transform.position =
            new Vector3
            (
                x ? player.transform.position.x : transform.position.x,
                y ? player.transform.position.y : transform.position.y,
                0
            );
        player.controller.rigid.velocity = Vector2.zero;
    }
}
