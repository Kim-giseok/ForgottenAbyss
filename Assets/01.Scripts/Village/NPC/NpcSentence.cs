using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NpcSentence : MonoBehaviour
{
    [TextArea]
    public string[] sentences;
    [SerializeField] UnityEvent displayEndEvents;
    
    public void TalkNpc()
    {
        StartCoroutine(DisplayEachSentence());
    }

    IEnumerator DisplayEachSentence()
    {
        GameManager.Instance.PausePlayer();

        foreach (var texts in sentences)
        {
            UIManager.Instance.OnTalk(transform, texts);
            
            yield return new WaitForSeconds(1f);
        }
        
        UIManager.Instance.OffTalk();
        displayEndEvents.Invoke();

        GameManager.Instance.PausePlayer(false);
    }
}
