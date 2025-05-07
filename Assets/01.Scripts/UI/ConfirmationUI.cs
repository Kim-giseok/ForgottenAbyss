using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationUI : BaseUI
{
    [SerializeField] TextMeshProUGUI confirmTxt;
    [SerializeField] Button okBtn;
    [SerializeField] Button exitBtn;
    Action onClick;

    private void Awake()
    {
        confirmTxt = GetComponentInChildren<TextMeshProUGUI>();
        okBtn.onClick.AddListener(ClickEnter);
        exitBtn.onClick.AddListener(() => gameObject.SetActive(false));
    }

    public void PopUpUI(string text = "입장하시겠습니까?", Action onClickOk = null)
    {
        gameObject.SetActive(true);
        confirmTxt.text = text;
        onClick = onClickOk;
    }

    void ClickEnter()
    {
        onClick?.Invoke();
        onClick = null;
        gameObject.SetActive(false);
    }
}
