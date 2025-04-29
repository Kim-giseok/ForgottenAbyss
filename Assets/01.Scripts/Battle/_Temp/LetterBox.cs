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
        if (currLine == 0) { StartCoroutine(ChangeSpacingOverTime(1200f, 860f, 1f)); }
    }
    
    private void Update()
    {
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {
            // if (!skipLine) { skipLine = true; return; }
            //
            // if(coroutine != null) StopCoroutine(coroutine);
            //
            // currLine += 1;
            // coroutine = StartCoroutine(Narration(currLine));
        }
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
    
    private IEnumerator ChangeSpacingOverTime(float startValue, float endValue, float duration)
    {
        float currTime = 0f;

        verticalLayoutGroup.spacing = startValue;

        while (currTime < duration)
        {
            float t = currTime / duration;
            verticalLayoutGroup.spacing = Mathf.Lerp(startValue, endValue, t);
            canvasGroup.alpha = Mathf.Lerp(0, 1, t);
            
            currTime += Time.deltaTime;
            yield return null;
        }
        verticalLayoutGroup.spacing = endValue;
    }
}