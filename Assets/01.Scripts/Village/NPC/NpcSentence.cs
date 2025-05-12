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
            Debug.Log(sentnece);
            UIManager.Instance.OnTalk(this, sentnece);
            yield return new WaitForSecondsRealtime(sentnece.Length * 0.1f);
        }
        UIManager.Instance.OffTalk();
        displayEndEvents.Invoke();

        GameManager.Instance.PausePlayer(false);
    }
}
