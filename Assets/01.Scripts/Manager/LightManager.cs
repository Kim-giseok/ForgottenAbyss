using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManager: SingletonLoadRemain<LightManager>
{
    public Light2D globalLight;
    public void FadeIn(float duration, float newIntensity = 1) => StartCoroutine(FadeLight(globalLight.intensity, newIntensity, duration));
    public void FadeOut(float duration, float newIntensity = 0) => StartCoroutine(FadeLight(globalLight.intensity, newIntensity, duration));

    private IEnumerator FadeLight(float from, float to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            globalLight.intensity = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }
        globalLight.intensity = to;
    }

    public void Reset()
    {
        globalLight.intensity = 1f;
        // error: 컬러 제대로 못가져오는 현상 발생
        globalLight.color = UnityEngine.Color.white;
    }

    public void Color(Color newColor)
    {
        globalLight.color = newColor;
    }
}