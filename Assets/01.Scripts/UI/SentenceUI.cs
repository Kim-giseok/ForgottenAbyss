using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SentenceUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI sentenceTxt;
    [SerializeField] RectTransform rect;

    public void Ondialogue(string sentence)
    {
        if (!gameObject.activeSelf) gameObject.SetActive(true);
        sentenceTxt.text = sentence;

        rect.sizeDelta = new Vector2(rect.sizeDelta.x, sentenceTxt.preferredHeight);
    }
}
