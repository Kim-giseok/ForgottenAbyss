using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManager: SingletonLoadRemain<LightManager>
{
    public Light2D globalLight;
    public void FadeIn(float duration) => StartCoroutine(FadeLight(globalLight.intensity, 1f, duration));
    public void FadeOut(float duration) => StartCoroutine(FadeLight(globalLight.intensity, 0f, duration));

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
}