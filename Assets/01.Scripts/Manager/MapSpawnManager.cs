using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawnManager : Singleton<MapSpawnManager>
{
    [SerializeField] private Map[] maps;
    public Map SpawnedMap { get; private set; }

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
        if (SpawnedMap != null)
            Destroy(SpawnedMap.gameObject);

        SpawnedMap = Instantiate(maps[Random.Range(0, maps.Length)]);
        SpawnedMap.MapStart();
    }
}
