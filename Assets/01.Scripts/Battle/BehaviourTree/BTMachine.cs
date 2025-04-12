using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BTMachine
{
    private EnemyController controller;
    private BlackBoard blackBoard = new();

    public bool isRunning = true;
    private Node rootNode;
    
    // currNode를 병렬으로 처리해서 노드 내 중복 코드 개선
    public List<Node> currNodes { get; private set; }
    // public List<(string name, Node Node)> allNodes = new(); // notice: 노드를 강제로 호출이 필요한 경우

    public BTMachine(EnemyController controller)
    {
        this.controller = controller;
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    public void Run()
    {
        if (!isRunning) return;
        currNodes.ForEach(node => node.Update());
    }

    public void SetNode(params Node[] newNodes)
    {
        isRunning = false;

        currNodes.ForEach(node => node?.End());
        
        currNodes.AddRange(newNodes);
        currNodes.ForEach(node =>
        {
            // node.SetController(controller); // notice: 바뀔 일이 없으니 맨 처음에 한번 등록하도록 변경 필요
            node.Start();
        });
        
        isRunning = true;
    }

    public void OnAnimatedEvent(bool isFire)
    {
        currNodes.ForEach(node => node.OnAnimatedEvent(isFire));
    }

    public void Define(Node newNode)    
    {
        rootNode = new RootNode(newNode);
        Connect(rootNode);
        SetNode(rootNode);
    }

    public void Connect(Node node)
    {
        node.SetController(controller);
        if (node.children.Count > 0) { node.children.ForEach(Connect); } // method group 기능
    }

    public void Notify() // 초기 노드로 이동 또는 특정 노드로 이동 기능 구현 필요
    {
        SetNode(rootNode);
    }

    public void Play(Node newNode, Action OnFinish)
    {
        rootNode = new RootNode(newNode).WhenLooped(OnFinish);
        SetNode(rootNode);
    }
}