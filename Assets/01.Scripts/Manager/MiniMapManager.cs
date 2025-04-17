using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapManager : MonoBehaviour
{
    public Camera miniMapCameraPrefab;
    public GameObject miniMapUIPrefab;

    public RenderTexture miniMapTexture;

    public Transform playerTarget;


    void Start()
    {
        // 카메라 생성
        Camera miniCam = Instantiate(miniMapCameraPrefab);
        DontDestroyOnLoad(miniCam.gameObject);
        miniCam.targetTexture = miniMapTexture;

        // UI생성
        GameObject miniMapUI = Instantiate(miniMapUIPrefab);
        DontDestroyOnLoad(miniMapUI);
        RawImage rawImage = miniMapUI.GetComponentInChildren<RawImage>();
        if (rawImage != null )
            rawImage.texture = miniMapTexture;

        // 타겟 연결
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            MiniMapFollow miniMapFollow = miniCam.GetComponent<MiniMapFollow>();
            if (miniMapFollow != null)
                miniMapFollow.target = playerObj.transform;
        }
    }
}
