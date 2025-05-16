using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Item item; // 아이템 데이터
    private Image image;
    private CanvasGroup canvasGroup; // 드래그할 때 UI 투명도 설정


    private void Awake()
    {
        EnsureInitialized();

        image = GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!Application.isPlaying)
            EnsureInitialized();
    }
#endif

    // 아이템 설정
    public void SetItem(Item newItem)
    {
        EnsureInitialized();

        item = newItem;

        if (item == null)
        {
            Debug.LogWarning("[ItemUI] SetItem()에 null 아이템 전달됨");
            return;
        }

        image.sprite = item.itemIcon;
        canvasGroup.alpha = 1f;
        gameObject.SetActive(true);

        Debug.Log($"[ItemUI] SetItem - SetActive(true) 호출됨: {gameObject.activeSelf}");
    }

    private void EnsureInitialized()
    {
        if (image == null) image = GetComponent<Image>();
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
    }

    // 아이템 제거
    public void RemoveItem()
    {
        Debug.Log($"[ItemUI] RemoveItem 호출됨");

        EnsureInitialized();

        item = null;

        if (image != null) image.sprite = null;
        if (canvasGroup != null) canvasGroup.alpha = 0f;

        // gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //if (item != null)
        //    UIManager.Instance.ShowTooltip(item, Input.mousePosition);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideTooltip();
    }
}


