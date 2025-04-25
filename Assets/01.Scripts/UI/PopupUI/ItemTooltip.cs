using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] private GameObject tooltipPanel;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descText;
    [SerializeField] private Image iconImage;

    private Vector3 targetPosition;    // 따라갈 목표 위치
    private RectTransform tooltipRect; // RectTransform 캐싱
    private RectTransform canvasRect;

    private void Awake()
    {
        tooltipRect = tooltipPanel.GetComponent<RectTransform>();
        //  Canvas를 기준으로 RectTransform 받아오기
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            canvasRect = canvas.GetComponent<RectTransform>();
        }
        else
        {
            Debug.LogError("ItemTooltip이 Canvas 내부에 있지 않습니다!");
        }
    }

    private void Update()
    {
        if (tooltipPanel.activeSelf)
        {
            tooltipRect.localPosition = Vector3.Lerp(
                tooltipRect.localPosition,
                targetPosition,
                Time.deltaTime * 10f // 속도값 클수록 빠름
            );
        }
    }

    public void Show(Item item, Vector3 screenPosition)
    {
        CanvasGroup group = tooltipPanel.GetComponent<CanvasGroup>();
        group.blocksRaycasts = false; // 이벤트 관통
        group.interactable = false;   // 클릭 안 됨

        if (item == null || canvasRect == null) return;

        tooltipPanel.SetActive(true);
        nameText.text = item.itemName;
        descText.text = item.itemDescription;
        iconImage.sprite = item.itemIcon;

        // 마우스 위치를 Canvas 기준 좌표로 변환
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPosition,
            null, // 카메라가 null이면 Overlay 모드일 때
            out localPoint
        );

        // 툴팁 위치 조정
        Vector2 offset = new Vector2(-tooltipRect.sizeDelta.x - 50f, -80f);
        targetPosition = localPoint + offset;
    }

    public void Hide()
    {
        tooltipPanel.SetActive(false);
    }
}
