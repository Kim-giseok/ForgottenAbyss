using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NpcSentence : MonoBehaviour
{
    public string[] sentences;
    [SerializeField] UnityEvent displayEndEvents;
    
    public void TalkNpc()
    {
        StartCoroutine(DisplayEachSentence());
    }

    IEnumerator DisplayEachSentence()
    {
        foreach (var sentnece in sentences)
        {
            Debug.Log(sentnece);
            UIManager.Instance.OnTalk(this, sentnece);
            yield return new WaitForSecondsRealtime(sentences.Length * 0.5f);
        }
        UIManager.Instance.OffTalk();
        displayEndEvents.Invoke();
    }
}
