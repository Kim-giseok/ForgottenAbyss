using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SentenceUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI sentenceTxt;
    [SerializeField] RectTransform rect;

    private bool isPressed;
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.anyKeyDown)
        {
            isPressed = true;       
        }
    }

    // notice: 인풋 담당을 컷신 매니저로 빼는 것은 어떨까?
    public async UniTask Set(string sentence)
    {
        if (!gameObject.activeSelf) gameObject.SetActive(true);

        sentenceTxt.text = "";
        isPressed = false;
        
        foreach (var text in sentence)
        {
            if (isPressed)
            {
                sentenceTxt.text = sentence;
                break;
            }

            sentenceTxt.text += text;
            SoundManager.Instance.Playsfx("Tick");
            await UniTask.Delay(40);
        }
    }
}