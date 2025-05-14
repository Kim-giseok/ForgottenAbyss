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

        foreach (var sentnece in sentences)
        {
            UIManager.Instance.OnTalk(transform, sentnece);
            
            while (!UIManager.Instance.talkBox.isFinished) { yield return null; }
            yield return new WaitForSeconds(0.6f);
        }
        
        UIManager.Instance.OffTalk();
        displayEndEvents.Invoke();

        GameManager.Instance.PausePlayer(false);
    }
}
