using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSpawnManager : Singleton<MapSpawnManager>
{
    [SerializeField] private Map[] maps;
    int mapIdx = 0;
    public Map SpawnedMap { get; private set; }

    private void Awake()
    {
        _instance = this;
        mapIdx = 0;
    }

    private void Start()
    {
        SpawnRandomMap();
    }

    public void SpawnRandomMap()
    {
        if (SpawnedMap != null)
            Destroy(SpawnedMap.gameObject);

        if (mapIdx >= maps.Length)
        {
            SceneManager.LoadScene(0); //이후 마을 신으로 수정
            return;
        }

        SpawnedMap = Instantiate(maps[mapIdx++]);
        SpawnedMap.MapStart();
    }
}
