using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BTMachine
{
    private EnemyBaseController controller;

    public BTContext context = new();
    public bool isPlaying { get; private set; } = false;
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
        if (!isPlaying) return;
        
        currTime += Time.fixedDeltaTime;
        
        var snapshot = new List<Node>(currNodes);
        snapshot.ForEach(node =>
        {
            node.SetController(controller);
            node.Update();
        });
    }

    public void SetPlaying(bool isPlaying)
    {
        this.isPlaying = isPlaying;
    }

    public void SetNode(params Node[] newNodes)
    {
        isPlaying = false;
        
        var snapshot = new List<Node>(newNodes);
        currNodes.ForEach(node =>
        {
            node.SetController(controller);
            node.End();
        });
        
        currTime = 0;
        currNodes.Clear();
        currNodes.AddRange(newNodes);
        
        snapshot = new List<Node>(newNodes);
        snapshot.ForEach(node =>
        {
            node.SetController(controller);
            node.Start();
        });
        
        isPlaying = true;
    }

    public void OnAnimatedEvent(bool isFire)
    {
        var snapshot = new List<Node>(currNodes); // 스냅샵 비용 발생
        // 이 작업 자체를 추상화하기
        snapshot.ForEach(node =>
        {
            node.SetController(controller); // 모든 이벤트마다 controller 등록이 발생함
            node.OnAnimatedEvent(isFire);
        });
    }

    public void OnASD()
    {
        var snapshot = new List<Node>(currNodes);
        snapshot.ForEach(node =>
        {
            node.SetController(controller);
            node.OnDetected(EnemyDetectHandler.DetectType.Grounded, true);
        });
    }

    public void Define(Node newNode)    
    {
        rootNode = new RootNode(newNode);
        // Connect(rootNode);
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
        // Connect(rootNode);
        SetNode(rootNode);
    }
}