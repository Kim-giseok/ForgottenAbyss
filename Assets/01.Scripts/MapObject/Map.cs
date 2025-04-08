using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] Transform startP;

    public void MapStart()
    {
        //move player's transform to startP
        try
        {
            GameManager.Instance.player.transform.position = startP.position;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
}
