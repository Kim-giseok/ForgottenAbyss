using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;

public class TalkSystem : MonoBehaviour
{
    public Queue<string> sentences;
    public string curSentence;
    public TextMeshPro NpcText;
    //public GameObject quad;
    SpriteRenderer spriteRenderer;
    PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }


    public void Ondialogue(string[] lines, Transform talkPoint)
    {
        transform.position = talkPoint.position;
        sentences = new Queue<string>();
        sentences.Clear();
        foreach (var line in lines)
        {
            sentences.Enqueue(line);
        }
        StartCoroutine(Dialogue(talkPoint));
    }

    IEnumerator Dialogue(Transform talkPoint)
    {
        yield return null;
        while (sentences.Count > 0)
        {
            playerInput.enabled = false;
            curSentence = sentences.Dequeue();
            NpcText.text = curSentence;
            spriteRenderer.size = new Vector2(NpcText.preferredWidth + 1.0f, NpcText.preferredHeight + 0.5f);
            transform.position = new Vector2(talkPoint.position.x, talkPoint.position.y);
            yield return new WaitForSeconds(2f);
            
        }
        gameObject.SetActive(false);
        playerInput.enabled = true;
    }
}
