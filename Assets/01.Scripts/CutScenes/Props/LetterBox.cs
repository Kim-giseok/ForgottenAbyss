using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 나레이션 컴포넌트로 분리하기
public class LetterBox : MonoBehaviour
{
    private VerticalLayoutGroup verticalLayoutGroup;
    private CanvasGroup canvasGroup;

    public TextMeshProUGUI narrationText;
    
    private CancellationTokenSource _narrationCTS;
    private Coroutine letterBoxCoroutine; 
    
    private bool skipLine;
    private int currLine;

    public float maxWidth;
    public float minWidth;
    public float duration;
    
    private void Awake()
    {
        verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {
            skipLine = true;
        }
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
    
    private async UniTask PlayNarrationAsync(string text)
    {
        if(!narrationText.gameObject.activeSelf) { narrationText.gameObject.SetActive(true); }
        
        skipLine = false;
        narrationText.text = "";
    
        foreach (var c in text)
        {
            _narrationCTS.Token.ThrowIfCancellationRequested();
            
            if (skipLine) { narrationText.text = text; break; }
            SoundManager.Instance.Playsfx("Tick");
            narrationText.text += c;
            await UniTask.Delay(30, cancellationToken: _narrationCTS.Token);
        }
    }

    public async UniTask Narration(string text)
    {
        _narrationCTS?.Cancel();
        
        if (text == "")
        {
            narrationText.gameObject.SetActive(false);
            return;
        }
        
        _narrationCTS = new CancellationTokenSource();
        await PlayNarrationAsync(text);
    }

    public void SetColor(Color newColor)
    {
        narrationText.color = newColor;
    }
}