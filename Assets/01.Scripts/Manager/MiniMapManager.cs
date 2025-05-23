using UnityEngine;
using UnityEngine.UI;

public class MiniMapManager : MonoBehaviour
{
    public Camera miniMapCameraPrefab;
    public RenderTexture miniMapTexture;
    public Transform playerTarget;

    public RawImage miniMapRawImage;


    void Start()
    {
        // 미니맵 카메라 생성 및 설정
        Camera miniCam = Instantiate(miniMapCameraPrefab);
        DontDestroyOnLoad(miniCam.gameObject);
        miniCam.targetTexture = miniMapTexture;

        // RawImage에 텍스처 연결
        if (miniMapRawImage != null)
            miniMapRawImage.texture = miniMapTexture;

        // 플레이어 타겟 설정
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            MiniMapFollow miniMapFollow = miniCam.GetComponent<MiniMapFollow>();
            if (miniMapFollow != null)
                miniMapFollow.target = playerObj.transform;
        }
    }
}
