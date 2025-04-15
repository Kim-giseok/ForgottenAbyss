using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraShake : Singleton<CameraShake>
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private CinemachineFramingTransposer framingTransposer;
    private Coroutine shakeCoroutine;

    void Awake()
    {
        if (virtualCamera != null)
        {
            framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
    }

    public void Shake(float duration = 0.1f, float amplitude = 0.2f, float frequency = 5f)
    {
        if (framingTransposer == null)
            return;

        // 기존 쉐이크가 진행 중이면 멈추기
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(ShakeRoutine(duration, amplitude, frequency));
    }

    private IEnumerator ShakeRoutine(float duration, float amplitude, float frequency)
    {
        if (framingTransposer != null)
        {
            framingTransposer.m_DeadZoneWidth = 0f;
            framingTransposer.m_DeadZoneHeight = 0f;

            var noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            if (noise != null)
            {
                noise.m_AmplitudeGain = amplitude;
                noise.m_FrequencyGain = frequency;
            }
        }

        yield return new WaitForSeconds(duration);

        if (framingTransposer != null)
        {
            var noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            if (noise != null)
            {
                noise.m_AmplitudeGain = 0f;
                noise.m_FrequencyGain = 0f;
            }
        }
    }
}
