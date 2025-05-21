using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] private GameObject tooltipPanel;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descText;
    [SerializeField] private Image iconImage;

    private RectTransform tooltipRect;
    private RectTransform canvasRect;

    private void Awake()
    {
        tooltipRect = tooltipPanel.GetComponent<RectTransform>();
        //  Canvas를 기준으로 RectTransform 받아오기
        Canvas canvas = GetComponentInParent<Canvas>();
        canvasRect = canvas?.GetComponent<RectTransform>();

        if (canvasRect == null)
            Debug.LogError("[Tooltip] Canvas RectTransform을 찾을 수 없습니다.");
    }

    public void SetData(ITooltipData data)
    {
        nameText.text = data.GetTitle();
        descText.text = data.GetDescription();
        iconImage.sprite = data.GetIcon();
    }

    public void Show(Vector2 screenPos)
    {
        if (canvasRect == null) return;

        tooltipPanel.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipRect);

        Vector2 tooltipSize = tooltipRect.rect.size;

        tooltipRect.pivot = new Vector2(0f, 1f);

        Vector2 offset = new Vector2(12f, -12f);
        Vector2 desiredScreenPos = screenPos + offset;

        float clampedX = Mathf.Clamp(desiredScreenPos.x, 0f, Screen.width - tooltipSize.x);
        float clampedY = Mathf.Clamp(desiredScreenPos.y, tooltipSize.y, Screen.height);
        Vector2 clampedScreenPos = new Vector2(clampedX, clampedY);

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, clampedScreenPos, null, out localPoint);
        tooltipRect.localPosition = localPoint;

        tooltipRect.SetAsLastSibling();
    }

    public void Hide()
    {
        tooltipPanel.SetActive(false);
    }
}
