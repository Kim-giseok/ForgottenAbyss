using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private CinemachineBasicMultiChannelPerlin noise;
    private Coroutine shakeCoroutine;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        if (virtualCamera == null)
        {
            Debug.LogWarning("CameraShake: 새 씬에서 VirtualCamera를 찾지 못했습니다.");
            return;
        }

        SetupNoiseComponent();
        RemoveExtraAudioListeners();
    }

    private void RemoveExtraAudioListeners()
    {
        var listeners = FindObjectsOfType<AudioListener>();
        if (listeners.Length > 1)
        {
            bool keptOne = false;
            foreach (var listener in listeners)
            {
                if (!keptOne)
                {
                    keptOne = true;
                    continue;
                }
                Destroy(listener);
            }

            Debug.LogWarning("[CameraShake] 중복 AudioListener가 감지되어 제거되었습니다.");
        }
    }

    private void SetupNoiseComponent()
    {
        noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (noise == null)
        {
            Debug.LogWarning("CameraShake: VirtualCamera에 Perlin Noise 컴포넌트가 없습니다!");
        }
    }

    public void Shake(float duration = 0.1f, float amplitude = 0.2f, float frequency = 5f)
    {
        if (noise == null)
        {
            Debug.LogWarning("CameraShake: noise 컴포넌트가 null입니다.");
            return;
        }

        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(ShakeRoutine(duration, amplitude, frequency));
    }

    private IEnumerator ShakeRoutine(float duration, float amplitude, float frequency)
    {
        noise.m_AmplitudeGain = amplitude;
        noise.m_FrequencyGain = frequency;

        yield return new WaitForSeconds(duration);

        noise.m_AmplitudeGain = 0f;
        noise.m_FrequencyGain = 0f;

        shakeCoroutine = null;
    }
}