using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationUI : BaseUI
{
    [SerializeField] Button okBtn;
    [SerializeField] Button exitBtn;
    Action onClick;

    private void Awake()
    {
        okBtn.onClick.AddListener(ClickEnter);
        exitBtn.onClick.AddListener(() => gameObject.SetActive(false));
    }

    public void PopUpUI(Action onClickOk)
    {
        gameObject.SetActive(true);
        onClick = onClickOk;
    }

    void ClickEnter()
    {
        onClick?.Invoke();
    }
}
