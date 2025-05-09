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
        GameManager.Instance.PausePlayer();

        foreach (var sentnece in sentences)
        {
            UIManager.Instance.OnTalk(this, sentnece);
            yield return new WaitForSecondsRealtime(sentnece.Length * 0.12f);
        }
        UIManager.Instance.OffTalk();
        displayEndEvents.Invoke();

        GameManager.Instance.PausePlayer(false);
    }
}
