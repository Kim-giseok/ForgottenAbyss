using System;
using TMPro;
using UnityEngine;

public class ToolTipComp: MonoBehaviour
{
    private RectTransform _rectTransform;
    private TextMeshProUGUI _textUI;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
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
        
        _rectTransform.anchoredPosition = currPos;
        _textUI.text = currText;
    }
}