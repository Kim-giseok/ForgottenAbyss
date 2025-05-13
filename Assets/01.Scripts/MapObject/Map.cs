using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] Transform startP;
    [field: SerializeField] public Collider2D CameraCollider { get; private set; }
    [field: SerializeField] public MonsterManager monsterManager { get; private set; }

    public void MapStart()
    {
        //move player's transform to startP
        try
        {
            GameManager.Instance.player.transform.position = startP.position;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }

        if (monsterManager == null)
            monsterManager = GetComponentInChildren<MonsterManager>();
    }
}
