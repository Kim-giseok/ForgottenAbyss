using System.Collections.Generic;
using UnityEngine;

// 각 노드 시간 등록 개념이 어려움
public class StepMachine
{
    public BoltBuilder bolt;
    
    public List<BoltNode> nodes = new();
    public BoltNode currNode;

    public float snapshotTime = 0f;
    public bool isRunning = false;
    
    // notice: bolt만 변경한다면 재사용하도록 가능한지 체크
    public StepMachine(BoltBuilder bolt) => this.bolt = bolt;

    // 전략 패턴으로 현재
    public void Run()
    {
        if (!isRunning) return;
        // 접근 노드 변경
        currNode.Connect(this);
        currNode.Update();
    }

    public void Stop()
    {
        isRunning = false;
        nodes.Clear();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void SetNode(int index)
    {
        currNode = nodes[index];
        
        currNode.Connect(this);
        currNode.Awake();
        currNode.Start();
        isRunning = true;
    }

    public void Next()
    {
        isRunning = false;
        
        if (currNode != null) { currNode?.End(); }
        
        int currIndex = nodes.IndexOf(currNode);
        
        // 종료인 경우 - destroy 할 것인지 체크 필요
        if (currIndex == nodes.Count - 1)
        {
            return;
        }
        
        SetNode(currIndex + 1);
    }

    // Start가 종속된 부분 변경 필요
    public void Define(BoltNode[] nodes)
    {
        this.nodes.Clear();
        this.nodes.AddRange(nodes);
    }

    // next로 하면 첫번째가 아닌 현상 발생
    public void Start() { SetNode(0); }

    public void Clear()
    {
        nodes.Clear();
    }
}