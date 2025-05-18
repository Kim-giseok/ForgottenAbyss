using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TempLetterBox : MonoBehaviour
{
    private VerticalLayoutGroup verticalLayoutGroup;
    private CanvasGroup canvasGroup;
    private AudioSource audioSource;

    public TextMeshProUGUI narrationText;
    
    private Coroutine narrationCoroutine; 
    private Coroutine letterBoxCoroutine; 
    
    private bool skipLine;
    private int currLine;

    [FormerlySerializedAs("maxWidth")] public float maxHieght;
    [FormerlySerializedAs("minWidth")] public float minHeight;
    public float duration;
    
    private bool isStartNarration;
    private bool isNarrationEnd;
    
    public GameObject agis;
    public GameObject agisActor;
    
    [TextArea(2, 2)] public string[] narrations;
    
    private void Awake()
    {
        verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
        canvasGroup = GetComponent<CanvasGroup>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        UIManager.Instance.HideIngameUI();
        
        letterBoxCoroutine = StartCoroutine(HandleWidth(true, () =>
        {
            if (narrations.Length != 0)
            { 
                narrationCoroutine = StartCoroutine(Narration(currLine));
                isStartNarration = true;
            }
        }));
    }
    
    private void Update()
    {
        if (narrations.Length == 0 || isNarrationEnd || !isStartNarration) return;
        
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {
            if (!skipLine) { skipLine = true; return; }

            if (currLine >= narrations.Length - 1)
            {
                isNarrationEnd = true;
                narrationText.text = "";
                StopCoroutine(letterBoxCoroutine);
                letterBoxCoroutine = StartCoroutine(HandleWidth(false, () => gameObject.SetActive(false)));
                return;
            }
            
            StopCoroutine(narrationCoroutine);
            currLine += 1;
            narrationCoroutine = StartCoroutine(Narration(currLine));
        }
    }

    private void OnDisable()
    {
        agis.SetActive(true);
        Destroy(agisActor);
        UIManager.Instance.ShowIngameUI();
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
    
    
    private IEnumerator HandleWidth(bool isShow, Action callback)
    {
        float currTime = 0f;
        
        float currStartWith = isShow ? maxHieght : minHeight;
        float currEndWith = isShow ? minHeight : maxHieght;
        
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
       callback?.Invoke();
        
    }
}