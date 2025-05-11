using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SceneGenerator
{
    private Light2D globalLight;
    private Camera mainCamera; // 카메라 흔들기, 플레이어 포커싱
    
    private GameObject TextUI; // 토크박스의 위치와 대사 진행
    private GameObject narrationUI; // 나레이션의 워딩
    
    // 레터박스 표시 여부, UIManager 숨김 여구
    public void SetNarrationUI() {}
}