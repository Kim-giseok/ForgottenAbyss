using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BTMachine
{
    private EnemyBaseController controller;

    
    public bool isRunning = true;
    public float currTime { get; private set; }

    private Node rootNode;
    // currNode를 병렬으로 처리해서 노드 내 중복 코드 개선
    public List<Node> currNodes { get; private set; } = new();

    public BTMachine(EnemyBaseController controller)
    {
        this.controller = controller;
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    public void Run()
    {
        if (!isRunning) return;
        
        currTime += Time.fixedDeltaTime;
        
        var snapshot = new List<Node>(currNodes);
        snapshot.ForEach(node => node.Update());
    }

    public void SetNode(params Node[] newNodes)
    {
        isRunning = false;
        
        var snapshot = new List<Node>(currNodes);
        // currNodes.ForEach(node => node.End());
        
        currTime = 0;
        currNodes.Clear();
        currNodes.AddRange(newNodes);
        
        snapshot.ForEach(node => node.Start());
        
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

    public void Notify() // 특정 노드로 이동 기능 구현 필요
    {
        SetNode(rootNode);
    }

    public void Play(Node newNode, Action OnFinish)
    {
        rootNode = new RootNode(newNode).WhenLooped(OnFinish);
        SetNode(rootNode);
    }
}