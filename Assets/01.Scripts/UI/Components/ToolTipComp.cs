using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ToolTipComp: MonoBehaviour
{
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private TextMeshProUGUI _textUI;
    
    public bool isActive { get; private set; } = false;
    
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _textUI = GetComponentInChildren<TextMeshProUGUI>();
    }
    
    private void OnDisable()
    {
        isActive = false;
        _textUI.text = "";
    }
    
    public void Shake(float duration = 0.3f, float strength = 30f, int vibrato = 20)
    {
        SoundManager.Instance.Playsfx("Wrong");
        _rectTransform.DOShakeAnchorPos(duration, new Vector2(strength, 0f), vibrato, randomness: 90, snapping: false, fadeOut: true);
    }

    public void On(bool isOn)
    {
        gameObject.SetActive(isOn);
    }

    public void Set(Vector3 currPos, string currText)
    {
        isActive = true;
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