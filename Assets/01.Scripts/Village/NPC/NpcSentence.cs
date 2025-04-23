using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcSentence : MonoBehaviour
{
    public string[] sentences;
    public Transform TalkPoint;
    public TalkSystem talkBoxPrefab;
    
    public void TalkNpc()
    {
        if (FindObjectOfType<TalkSystem>() != null)
        {
            return; // 이미 대화창이 있으면 더 이상 생성X
        }

        TalkSystem gameObject = Instantiate(talkBoxPrefab);
        gameObject.Ondialogue(sentences, TalkPoint);
    }
}
