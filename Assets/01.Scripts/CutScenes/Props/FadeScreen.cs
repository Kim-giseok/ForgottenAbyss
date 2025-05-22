using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreen: MonoBehaviour
{
    private Image image;

    // 맨처음 등록 안되는 부분으로 인해 체크
    private void Awake()
    {
        image = GetComponent<Image>();
        gameObject.SetActive(false);
    }

    public void SetAlpha(float alpha)
    {
        gameObject.SetActive(alpha != 0);

        var newColor = image.color;
        newColor.a = alpha;
        image.color = newColor;
    }

    public void SetFade(bool isFadeIn, float duration)
    {
        gameObject.SetActive(true);
        Fade(isFadeIn, duration).Forget();
    }

    private async UniTask Fade(bool isFadeIn, float duration)
    {
        float currTime = 0;
        Color currColor = image.color;
        
        float startAlpha = isFadeIn ? 1f : 0f;
        float endAlpha = isFadeIn ? 0f : 1f;

        while (currTime < duration) 
        {
            float rate = currTime / duration;
            currColor.a = Mathf.Lerp(startAlpha, endAlpha, rate);
            image.color = currColor;
            currTime += Time.deltaTime;
            await UniTask.Yield();
        }

        currColor.a = endAlpha;
        image.color = currColor;
    }

    public void Reset()
    {
        gameObject.SetActive(false);
    }
}