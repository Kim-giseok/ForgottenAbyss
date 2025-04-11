using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSpawnManager : Singleton<MapSpawnManager>
{
    [SerializeField] private Map[] maps;
    [SerializeField] CinemachineConfiner2D confiner2D;
    int mapIdx = 0;
    public Map SpawnedMap { get; private set; }

    private void Awake()
    {
        _instance = this;
        mapIdx = 0;
    }

    private void Start()
    {
        CinemachineVirtualCamera virtualCamera = confiner2D.GetComponent<CinemachineVirtualCamera>();
        virtualCamera.Follow = GameManager.Instance.player.transform;
        SpawnRandomMap();
    }

    public void SpawnRandomMap()
    {
        if (mapIdx >= maps.Length)
        {
            SceneManager.LoadScene(0); //이후 마을 신으로 수정
            return;
        }

        if (SpawnedMap != null)
            Destroy(SpawnedMap.gameObject);

        SpawnedMap = Instantiate(maps[mapIdx++]);
        SpawnedMap.MapStart();
        confiner2D.m_BoundingShape2D = SpawnedMap.CameraCollider;
    }
}
