using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LetterBox : MonoBehaviour
{
    private VerticalLayoutGroup verticalLayoutGroup;
    private CanvasGroup canvasGroup;

    public TextMeshProUGUI narrationText;
    
    private Coroutine narrationCoroutine; 
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

    public void ShowLetterBox(bool isShow)
    {
        if(letterBoxCoroutine != null) StopCoroutine(letterBoxCoroutine);
        if(isShow) gameObject.SetActive(true);
        letterBoxCoroutine = StartCoroutine(HandleWidth(isShow));
    }
    
    // private IEnumerator Narration(int lineIndex)
    // {
    //     skipLine = false;
    //     narrationText.text = "";
    //     
    //     string line = narrations[lineIndex];
    //
    //     foreach (char c in line)
    //     {
    //         if (skipLine) { narrationText.text = line; break; }
    //         
    //         audioSource.PlayOneShot(audioSource.clip);
    //         narrationText.text += c;
    //         yield return new WaitForSeconds(0.1f);
    //     }
    //     
    //     skipLine = true;
    // }
    
    
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
}