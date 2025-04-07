using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawnManager : Singleton<MapSpawnManager>
{
    [SerializeField] private Map[] maps;
    Map curMap;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        SpawnRandomMap();
    }

    public void SpawnRandomMap()
    {
        if (curMap != null)
            Destroy(curMap.gameObject);

        curMap = Instantiate(maps[Random.Range(0, maps.Length)]);
    }
}
