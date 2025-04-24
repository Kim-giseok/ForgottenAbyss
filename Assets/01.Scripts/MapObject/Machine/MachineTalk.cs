using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineTalk : MachineLoop
{
    [SerializeField] string sentence;
    [SerializeField] float delayTime;

    public override void Active()
    {
        base.Active();
        DisplaySentence();
    }

    void DisplaySentence()
    {
        StartCoroutine(CoroutineDisplay());
    }

    IEnumerator CoroutineDisplay()
    {
        Time.timeScale = 0f;
        string[] sentences = sentence.Split("\\n");
        foreach (var txt in sentences)
        {
            Debug.Log(txt);
            yield return new WaitForSecondsRealtime(delayTime);
        }
        Time.timeScale = 1f;
    }
}
