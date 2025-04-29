using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LetterBox : MonoBehaviour
{
    private VerticalLayoutGroup verticalLayoutGroup;
    private CanvasGroup canvasGroup;
    private AudioSource audioSource;

    public TextMeshProUGUI narrationText;
    private Coroutine coroutine; 
    
    private bool skipLine;
    private int currLine;

    public float startWidth;
    public float endWidth;
    public float duration;

    [TextArea(2, 2)] public string[] narrations;
    
    private void Awake()
    {
        verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
        canvasGroup = GetComponent<CanvasGroup>();
        
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        coroutine = StartCoroutine(Narration(currLine));
        if (currLine == 0) { StartCoroutine(HandleWidth(false)); }
    }
    
    private void Update()
    {
    }
    
    private IEnumerator Narration(int lineIndex)
    {
        skipLine = false;
        narrationText.text = "";
        
        string line = narrations[lineIndex];
    
        foreach (char c in line)
        {
            if (skipLine) { narrationText.text = line; break; }
            
            audioSource.PlayOneShot(audioSource.clip);
            narrationText.text += c;
            yield return new WaitForSeconds(0.1f);
        }
        
        skipLine = true;
    }
    
    
    private IEnumerator HandleWidth(bool isShow)
    {
        float currTime = 0f;
        
        float currStartWith = isShow ? startWidth : endWidth;
        float currEndWith = isShow ? endWidth : startWidth;
        
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
        verticalLayoutGroup.spacing = currEndWith;
    }
}