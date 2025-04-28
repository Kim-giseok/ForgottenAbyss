using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NarrationUI: MonoBehaviour
{
    public Text narrationText;
    private Coroutine coroutine; 
    
    private bool isTyping = false;
    private bool skipLine = false;
    
    private int currLine = 0;

    private string[] narrations =
    {
        "태초에 그림자가 있었다.\n 그림자는 실체가 없는 에너지 자체이다.", 
        "하지만 많은 약한 영혼들을 잠식하여\n 그림자는 인격화되기 시작했다.",
        "전쟁은 인간의 절망이자 그림자만의 기쁨이었고,",
        "혼란 속에서 점령되어\n 그림자의 에너지 공급자로 이용당했다.",
        "우리는 이 곳을 잊혀진 나락이라고 부른다."
    };
    
    private void Start()
    {
        coroutine = StartCoroutine(Narration(currLine));
    }
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StopCoroutine(coroutine);
            coroutine = StartCoroutine(Narration(++currLine));
        }
    }
    
    private IEnumerator Narration(int lineIndex)
    {
        narrationText.text = "";
        string line = narrations[lineIndex];
    
        foreach (char c in line)
        {
            narrationText.text += c;
            // if (skipLine) 
            // {
            //     narrationText.text = line;
            //     break;
            // }
            yield return new WaitForSeconds(0.1f);
        }
    }
}