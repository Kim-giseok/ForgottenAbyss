using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWarp : MonoBehaviour
{
    public void WarpPlayerToThis()
    {
        Player player = GameManager.Instance.player;

        player.transform.position = transform.position;
        player.controller.rigid.velocity = Vector2.zero;
    }
}
