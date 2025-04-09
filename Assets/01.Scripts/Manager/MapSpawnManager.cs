using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawnManager : Singleton<MapSpawnManager>
{
    [SerializeField] private Map[] maps;
    public Map CurMap { get; private set; }

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
        if (CurMap != null)
            Destroy(CurMap.gameObject);

        CurMap = Instantiate(maps[Random.Range(0, maps.Length)]);
        CurMap.MapStart();
    }
}
