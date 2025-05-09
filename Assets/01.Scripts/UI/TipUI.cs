using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TipUI: MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descText;
    
    [System.Serializable] public class Tip { public string title; public string desc; }
    public List<Tip> tips;

    public int currIndex;
    private void Shuffle()
    {
        int newIndex;
        do { newIndex = Random.Range(0, tips.Count); }
        while (newIndex == currIndex);
        currIndex = newIndex;
       
        Tip currTip = tips[currIndex];
        titleText.text = currTip.title;
        descText.text = currTip.desc;
    }

    private void Start()
    {
      Shuffle();
    }
    
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            Shuffle();
        }
    }
}