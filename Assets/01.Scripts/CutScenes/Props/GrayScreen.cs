using Cysharp.Threading.Tasks;
using UnityEngine;

public class GrayScreen: MonoBehaviour
{
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }

    public async UniTask Set(bool expand, float duration)
    {
        if(!gameObject.activeSelf) gameObject.SetActive(true);
        
        float startWidth = expand ? 0f : 1920f;
        float endWidth = expand ? 1920f : 0f;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float rate = elapsed / duration;
            float newWidth = Mathf.Lerp(startWidth, endWidth, rate);
            rectTransform.sizeDelta = new Vector2(newWidth, rectTransform.sizeDelta.y);

            elapsed += Time.deltaTime;
            await UniTask.Yield();
        }

        rectTransform.sizeDelta = new Vector2(endWidth, rectTransform.sizeDelta.y);
    }
}