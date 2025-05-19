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

    private void Awake()
    {
        if (virtualCamera != null)
            SetupNoiseComponent();
    }

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
        StartCoroutine(WaitForCamera());

        SetupNoiseComponent();

        RemoveExtraAudioListeners();
    }

    private IEnumerator WaitForCamera()
    {
        yield return new WaitForSeconds(0.1f); // 짧은 시간 기다린 후 다시 시도

        virtualCamera = MapSpawnManager.Instance?.virtualCamera;

        if (virtualCamera == null)
        {
            Debug.LogWarning("CameraShake: VirtualCamera를 찾을 수 없습니다.");
        }
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

            Debug.LogWarning("[CameraShake] �ߺ� AudioListener�� �����Ǿ� ���ŵǾ����ϴ�.");
        }
    }

    private void SetupNoiseComponent()
    {
        noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (noise == null)
        {
            Debug.LogWarning("CameraShake: VirtualCamera�� Perlin Noise ������Ʈ�� �����ϴ�!");
        }
    }

    public void Shake(float duration = 0.1f, float amplitude = 0.2f, float frequency = 5f)
    {
        if (noise == null)
        {
            Debug.LogWarning("CameraShake: noise ������Ʈ�� null�Դϴ�.");
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