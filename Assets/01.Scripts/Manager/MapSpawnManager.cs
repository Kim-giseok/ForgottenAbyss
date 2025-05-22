using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[Serializable]
public class StageTimeInfo
{
    public List<float> stageEnterTime;
    public float stageClearTime;
}

public class MapSpawnManager : Singleton<MapSpawnManager>
{
    [SerializeField] FLAGKEY stageClearedFlag;
    [SerializeField] private Map[] maps;
    [SerializeField] CinemachineConfiner2D confiner2D;
    public CinemachineVirtualCamera virtualCamera { get; private set; }
    
    int mapIdx = 0;
    public Map SpawnedMap { get; private set; }

    public Sprite[] backgroundSprites;
    public BackGround backgroundprefeb;
    public BackGround[] backgrounds;

    StageTimeInfo timeInfo = new();

    protected override void Awake()
    {
        base.Awake();
        mapIdx = 0;

        timeInfo.stageEnterTime = new();
    }

    private void Start()
    {
        virtualCamera = confiner2D.GetComponent<CinemachineVirtualCamera>();
        virtualCamera.Follow = GameManager.Instance.player.transform;

        SpawnBackground();
        SpawnNextMap();
    }

    private void LateUpdate()
    {
        ParrallexScrollBackground();
    }

    public void SpawnNextMap()
    {
        if (mapIdx >= maps.Length)
        {
            ActivateFlag.ActiveFlag(stageClearedFlag);
            timeInfo.stageClearTime = Time.time - timeInfo.stageEnterTime[0];
            DataSave<StageTimeInfo>.SaveData(timeInfo, stageClearedFlag.ToString() + ".json");

            SceneLoader.Instance.LoadScene("Village"); //���� ���� ������ ����
            return;
        }

        if (SpawnedMap != null)
            Destroy(SpawnedMap.gameObject);
        foreach (var item in GameObject.FindGameObjectsWithTag("FieldItem"))
        { item.gameObject.SetActive(false); }

        SpawnedMap = Instantiate(maps[mapIdx++]);

        SpawnedMap.MapStart();
        confiner2D.m_BoundingShape2D = SpawnedMap.CameraCollider;

        timeInfo.stageEnterTime.Add(Time.time);
    }

    void SpawnBackground()
    {
        backgrounds = new BackGround[backgroundSprites.Length];
        for (int i = 0; i < backgroundSprites.Length; i++)
        {
            backgrounds[i] = Instantiate(backgroundprefeb, Vector3.zero, Quaternion.identity);
            backgrounds[i].sprite = backgroundSprites[i];
            backgrounds[i].sortingOrder = -100 + i;
        }
    }

    void ParrallexScrollBackground()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float posX = (SpawnedMap.transform.position - Camera.main.transform.position).x * i / backgrounds.Length;
            Vector3 newPosition = Camera.main.transform.position - Vector3.left * posX;
            backgrounds[i].position = new Vector3(newPosition.x, newPosition.y);
        }
    }
}
