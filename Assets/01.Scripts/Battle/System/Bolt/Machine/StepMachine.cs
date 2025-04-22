using System.Collections.Generic;
using UnityEngine;

// 각 노드 시간 등록 개념이 어려움
public class StepMachine
{
    public Bolt bolt;
    
    public List<BoltNode> nodeList = new();
    public BoltNode currNode;


    public bool isRunning = false;
    
    // notice: bolt만 변경한다면 재사용하도록 가능한지 체크
    public StepMachine(Bolt bolt) => this.bolt = bolt;

    // 전략 패턴으로 현재
    public void Run()
    {
        if (!isRunning) return;
        // 접근 노드 변경
        currNode.Connect(this);
        currNode.Update();
    }

    //
    public void Next()
    {
        isRunning = false;
        
        if (currNode != null) { currNode?.End(); }
        
        int currIndex = nodeList.IndexOf(currNode);
        
        // 종료인 경우 체크 필요
        if (currIndex == nodeList.Count - 1)
        {
            return;
        }
        
        currNode = nodeList[currIndex + 1];
        
        currNode.Connect(this);
        currNode.Start();
        
        isRunning = true;
    }

    public void Define(BoltNode[] nodes)
    {
        nodeList.AddRange(nodes);
        
        // next로 하면 첫번째가 아닌 현상 발생
        currNode = nodes[0];
        currNode.Connect(this);
        currNode.Start();
        isRunning = true;
    }

    public void Clear()
    {
        nodeList.Clear();
    }
}