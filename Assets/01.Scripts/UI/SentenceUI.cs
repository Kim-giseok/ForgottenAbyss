using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SentenceUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI sentenceTxt;
    [SerializeField] RectTransform rect;
    Coroutine typingCoroutine;

    public void Ondialogue(string sentence)
    {
        if (!gameObject.activeSelf) gameObject.SetActive(true);

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        sentenceTxt.text = "";
        for (int i = 0; i < sentence.Length; i++)
        {
            sentenceTxt.text += sentence[i];
            // rect.sizeDelta = new Vector2(rect.sizeDelta.x, sentenceTxt.preferredHeight);
            SoundManager.Instance.Playsfx("Tick");
            yield return new WaitForSecondsRealtime(0.06f);
        }
    }
}
