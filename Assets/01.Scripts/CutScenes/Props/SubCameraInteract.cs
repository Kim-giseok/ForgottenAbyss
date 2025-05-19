using System;
using System.Collections.Generic;
using Cinemachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class SubCameraInteract: MonoBehaviour
{
    private CinemachineBrain cameraBrain;
    
    public CinemachineVirtualCamera virCam1;
    public CinemachineVirtualCamera virCam2;

    private CinemachineBasicMultiChannelPerlin virCam1Noise;
    private CinemachineBasicMultiChannelPerlin virCam2Noise;
    
    public enum NoiseType { Base, Held }
    public NoiseSettings defaultNoise;
    public NoiseSettings handHeldNoise;
    private Dictionary<int, NoiseSettings> noiseProfiles;

    public CinemachineVirtualCamera ActiveCam { get; private set; }
    
    private CinemachineConfiner2D confiner2D;
    private CinemachineConfiner2D virCam1Confiner2D;
    private CinemachineConfiner2D virCam2Confiner2D;
    
    private Tween currentZoomTween;
    private readonly float defaultFOV = 5f;

    private void Awake()
    {
        virCam1Noise = virCam1.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        virCam2Noise = virCam2.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        
        noiseProfiles = new Dictionary<int, NoiseSettings>
        {
            { (int)NoiseType.Base, defaultNoise },
            { (int)NoiseType.Held, handHeldNoise }
        };
    }

    private void Start()
    {
        ActiveCam = virCam1;
    }

    // error: awake에서 실행할 경우, 타이틀 씬 등에서 문제 발생
    public void Init()
    {
        cameraBrain = FindObjectOfType<CinemachineBrain>();
        
        confiner2D = MapSpawnManager.Instance.virtualCamera.GetComponent<CinemachineConfiner2D>();
        
        virCam1Confiner2D = virCam1.GetComponent<CinemachineConfiner2D>();
        virCam2Confiner2D = virCam2.GetComponent<CinemachineConfiner2D>();
        
        virCam1Confiner2D.m_BoundingShape2D = confiner2D?.m_BoundingShape2D;
        virCam2Confiner2D.m_BoundingShape2D = confiner2D?.m_BoundingShape2D;
    }

    public void DisConnect()
    {
        virCam1Confiner2D.enabled = false;
        virCam2Confiner2D.enabled = false;
    }
    
    private void SetUp()
    {
        cameraBrain.m_DefaultBlend.m_Time = 0.6f;

        ActiveCam.Follow = null;
        ActiveCam.LookAt = null;
        ActiveCam.Priority = 11;
    }

    public void Reset()
    {
        // cameraBrain.m_DefaultBlend.m_Time = 0.0f;
        
        virCam1.Priority = 0;
        virCam2.Priority = 0;

        virCam1.Follow = null;
        virCam1.LookAt = null;
        
        virCam2.Follow = null;
        virCam2.LookAt = null;
        
        Noise(0);
        Profile(NoiseType.Base);
    }
    
    public void Focus(Vector3 targetPos)
    {
        SetUp();
        CinemachineVirtualCamera nextCam = (ActiveCam == virCam1) ? virCam2 : virCam1;
        
        nextCam.transform.position = targetPos;
        nextCam.Priority = 12;

        ActiveCam = nextCam;
    }
    
    public void Focus(Player target)
    {
        Focus(target.transform);
    }
    
    public void Focus(GameObject target)
    {
        Focus(target.transform);
    }
    
    public void Focus(Transform target)
    {
        SetUp();
        CinemachineVirtualCamera nextCam = (ActiveCam == virCam1) ? virCam2 : virCam1;
        
        nextCam.Follow = target;
        nextCam.LookAt = target;
        nextCam.Priority = 12;

        ActiveCam = nextCam;
    }
    
    public void Profile(NoiseType newNoiseType)
    {
        virCam1Noise.m_NoiseProfile = noiseProfiles[(int)newNoiseType];
        virCam2Noise.m_NoiseProfile = noiseProfiles[(int)newNoiseType];
    }

    public void Noise(float amplitudeGain, float frequencyGain)
    {
        
        virCam1Noise.m_AmplitudeGain = amplitudeGain;
        virCam1Noise.m_FrequencyGain = frequencyGain;
        
        virCam2Noise.m_AmplitudeGain = amplitudeGain;
        virCam2Noise.m_FrequencyGain = frequencyGain;
    }

    public void Noise(float gain)
    {
        virCam1Noise.m_AmplitudeGain = gain;
        virCam1Noise.m_FrequencyGain = gain;
        
        virCam2Noise.m_AmplitudeGain = gain;
        virCam2Noise.m_FrequencyGain = gain;
    }
    
    public void Shake(float amplitude, float frequency, float duration)
    {
        ShakeCameraAsync(amplitude, frequency, duration).Forget();
    }

    private async UniTask ShakeCameraAsync(float amplitude, float frequency, float duration)
    {
        try
        {
            Noise(amplitude, frequency);
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            Noise(0f);
        }
        catch (Exception e)
        {
            Debug.LogError($"[subCams Shake] Error: {e}");
        }
    }
    
    public void Zoom(bool zoomIn, float duration = 0.5f, float targetSize = 3f)
    {
        if (currentZoomTween != null && currentZoomTween.IsActive()) currentZoomTween.Kill();

        var cam = ActiveCam;
        float from = cam.m_Lens.OrthographicSize;
        float to = zoomIn ? targetSize : defaultFOV;

        currentZoomTween = DOTween.To(() => from, x => cam.m_Lens.OrthographicSize = x, to, duration).SetEase(Ease.InOutSine);
    }
}