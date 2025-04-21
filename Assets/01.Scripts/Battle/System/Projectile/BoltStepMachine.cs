using System.Collections.Generic;

// 순차 실행이므로 선언 자체를 
public class BoltStepMachine
{
    public BoltController bot;
    
    public List<BoltNode> nodeList;
    public BoltNode currNode;

    public float currTime = 0f;

    // 전략 패턴으로 현재
    public void Run()
    {
    }

    //
    public void Next()
    {
        if (currNode != null) { currNode?.End(); }
        int currIndex = nodeList.IndexOf(currNode);
        // 종료인 경우 체크 필요
        currNode = nodeList[currIndex + 1];
        currNode.Start();
    }
}