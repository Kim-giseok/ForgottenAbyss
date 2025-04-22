using System.Collections.Generic;

// 각 노드 시간 등록 개념이 어려움
public class StepMachine
{
    public Bolt bolt;
    
    public List<BoltNode> nodeList;
    public BoltNode currNode;


    public bool isRunning = false;
    public float currTime = 0f;

    // 전략 패턴으로 현재
    public void Run()
    {
        if (!isRunning) return;
        currNode.Update();
    }

    //
    public void Next()
    {
        if (currNode != null) { currNode?.End(); }
        int currIndex = nodeList.IndexOf(currNode);
        // 종료인 경우 체크 필요
        currNode = nodeList[currIndex + 1];
        currNode.Connect(this);
        currNode.Start();
    }
}