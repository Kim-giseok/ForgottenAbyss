using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ToolTipComp: MonoBehaviour
{
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private TextMeshProUGUI _textUI;
    
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _textUI = GetComponentInChildren<TextMeshProUGUI>();
    }
    
    private void OnDisable()
    {
        _textUI.text = "";
    }

    public void On(bool isOn)
    {
        gameObject.SetActive(isOn);
    }

    public void Set(Vector3 currPos, string currText)
    {
        gameObject.SetActive(true);
        
        // RectTransformUtility.ScreenPointToLocalPointInRectangle()
        // Vector3 screenPos = Camera.main!.WorldToScreenPoint(currPos);
        // _rectTransform.transform.position = screenPos;
        
        _textUI.text = currText;
        
        _rectTransform.anchoredPosition = currPos + Vector3.up * 100f;
        _canvasGroup.alpha = 0;

        _rectTransform.DOAnchorPos(currPos, 0.4f).SetEase(Ease.OutCubic);
        _canvasGroup.DOFade(1f, 0.4f);
    }
}