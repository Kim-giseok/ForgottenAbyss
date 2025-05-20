using Cysharp.Threading.Tasks;
using UnityEngine;

public class CutUIPool: MonoBehaviour
{
    private CanvasGroup group;

    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
    }
    
    public void Set(RectTransform newUIComp, Vector3 newPos = default)
    {
        if(newUIComp.parent != transform) { newUIComp.transform.SetParent(transform); }
        newUIComp.anchoredPosition = newPos == default ? Vector3.zero : newPos;
    }

    public void Delete(RectTransform newUIComp)
    {
        newUIComp.gameObject.SetActive(false);
    }
    
    public async UniTask Fade(bool isActive, float duration)
    {
        group.alpha = 0;
        group.gameObject.SetActive(true);
        group.interactable = true;
        group.blocksRaycasts = true;
        
        float startAlpha = isActive ? 0f : 1f;
        float endAlpha = isActive ? 1f : 0f;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            group.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            elapsed += Time.deltaTime;
            await UniTask.Yield();
        }
        group.alpha = endAlpha;
    }
}