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
    public bool isTyping { get; private set; }
    public bool skipTyping { get; private set; }
    public bool isFinished { get; private set; }

    private void Update()
    {
        if (isTyping && Input.GetMouseButtonDown(0))
        {
            skipTyping = true;
        }
    }
    
    public void Ondialogue(string sentence)
    {
        if (!gameObject.activeSelf) gameObject.SetActive(true);

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);

        // 새로운 대사 시작 시 상태 초기화
        isFinished = false;
        skipTyping = false;
        typingCoroutine = StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        sentenceTxt.text = "";
        isTyping = true;
        
        for (int i = 0; i < sentence.Length; i++)
        {
            if (skipTyping)
            {
                sentenceTxt.text = sentence;
                break;
            }

            sentenceTxt.text += sentence[i];
            SoundManager.Instance.Playsfx("Tick");
            yield return new WaitForSecondsRealtime(0.06f);
        }

        isTyping = false;
        isFinished = true;
    }
}