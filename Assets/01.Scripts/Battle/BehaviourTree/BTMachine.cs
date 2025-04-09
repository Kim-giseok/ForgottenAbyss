using UnityEngine;

public class BTMachine
{
    private EnemyController controller;
    private BlackBoard blackBoard = new();

    public bool isRunning = true;
    private Node rootNode;
    public Node currNode { get; private set; }

    public BTMachine(EnemyController controller)
    {
        this.controller = controller;
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    public void Run()
    {
        if (!isRunning) return;
        currNode.Update();
    }

    public void SetNode(Node newNode)
    {
        isRunning = false;
        
        currNode?.End();
        currNode = newNode;
        currNode.Connect(controller);
        currNode.Start();
        
        isRunning = true;
    }

    public void OnAnimatedEvent(bool isFire)
    {
        // Debug.Log(currNode);
        currNode?.OnAnimatedEvent(isFire);
    }

    public void Define(Node newNode)    
    {
        rootNode = new RootNode(newNode);
        SetNode(rootNode);
    }

    public void Notify() // 초기 노드로 이동 또는 특정 노드로 이동 기능 구현 필요
    {
        SetNode(rootNode);
    }
}