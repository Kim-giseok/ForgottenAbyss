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

        tooltipRect.pivot = new Vector2(0f, 1f);
        tooltipRect.position = screenPos;
    }

    public void Hide()
    {
        tooltipPanel.SetActive(false);
    }
}
