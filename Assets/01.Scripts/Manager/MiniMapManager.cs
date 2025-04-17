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
        // 미니맵 카메라 생성
        Camera miniCam = Instantiate(miniMapCameraPrefab);
        miniCam.targetTexture = miniMapTexture;

        // 미니맵 UI 생성
        GameObject ui = Instantiate(miniMapUIPrefab);

        // UI 로우이미지에 텍스처 연결
        RawImage raw = ui.GetComponentInChildren<RawImage>();
        if (raw != null)
        {
            raw.texture = miniMapTexture;
        }

        // 카메라가 타겟 따라다니도록 설정
        MiniMapFollow follow = miniCam.GetComponent<MiniMapFollow>();
        if (follow != null)
        {
            follow.target = playerTarget;
        }
    }
}
