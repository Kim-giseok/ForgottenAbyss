using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Player player;

    private void Awake()
    {
        _instance = this;
        player = FindObjectOfType<Player>();
    }
}
