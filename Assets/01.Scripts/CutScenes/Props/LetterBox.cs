using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LetterBox : MonoBehaviour
{
    private VerticalLayoutGroup verticalLayoutGroup;
    private CanvasGroup canvasGroup;

    public TextMeshProUGUI narrationText;
    
    private Coroutine letterBoxCoroutine; 
    
    private bool skipLine;
    private int currLine;

    public float maxWidth;
    public float minWidth;
    public float duration;
    
    private bool isStartNarration;
    private bool isNarrationEnd;

    public UnityEvent OnNarrationStarted;
    public UnityEvent OnNarrationEnd;
    
    [TextArea(2, 2)] public string[] narrations;
    
    private void Awake()
    {
        verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    
    private IEnumerator HandleWidth(bool isShow)
    {
        float currTime = 0f;
        
        float currStartWith = isShow ? maxWidth : minWidth;
        float currEndWith = isShow ? minWidth : maxWidth;
        
        float currStartAlpha = isShow ? 0f : 1f;
        float currEndAlpha = isShow ? 1f : 0f;

        verticalLayoutGroup.spacing = currStartWith;

        while (currTime < duration)
        {
            float t = currTime / duration;
            verticalLayoutGroup.spacing = Mathf.Lerp(currStartWith, currEndWith, t);
            canvasGroup.alpha = Mathf.Lerp(currStartAlpha, currEndAlpha, t);
            
            currTime += Time.deltaTime;
            yield return null;
        }
        
        yield return new WaitForSeconds(1f);

        verticalLayoutGroup.spacing = currEndWith;
        if(!isShow) gameObject.SetActive(false);
    }

    public void Set(bool isShow)
    {
        if(letterBoxCoroutine != null) StopCoroutine(letterBoxCoroutine);
        if(isShow) gameObject.SetActive(true);
        letterBoxCoroutine = StartCoroutine(HandleWidth(isShow));
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.anyKeyDown)
        {
            skipLine = true;
        }
    }

    public async UniTask SetNarration(string text)
    {
        if(!narrationText.gameObject.activeSelf) { narrationText.gameObject.SetActive(true); }
        
        skipLine = false;
        narrationText.text = "";
    
        foreach (char c in text)
        {
            if (skipLine) { narrationText.text = text; break; }
            
            SoundManager.Instance.Playsfx("Tick");
            narrationText.text += c;
            await UniTask.Delay(30);
        }
    }

    public void SetNarrationColor(Color newColor)
    {
        narrationText.color = newColor;
    }
}