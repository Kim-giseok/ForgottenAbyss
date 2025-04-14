using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BTMachine
{
    private EnemyBaseController controller;
    public BTContext context = new();
    
    public Action OnLooped = delegate { };
    
    public bool isPlaying { get; private set; } = false;
    public float currTime = 0;

    // currNode를 병렬으로 처리해서 노드 내 중복 코드 개선
    public Node currNode { get; private set; }

    public BTMachine(EnemyBaseController controller)
    {
        this.controller = controller;
    }
    
    public void Run()
    {
        if (!isPlaying) return;

        currTime += Time.fixedDeltaTime;
        currNode.SetController(controller);
        currNode.Update();
    }

    public void SetPlaying(bool isPlaying)
    {
        this.isPlaying = isPlaying;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void SetCurrentNode(Node node)
    {
        SetPlaying(false);
        
        currNode?.End();

        currTime = 0;
        currNode = node;
        currNode.SetController(controller);
        currNode.Start();
        
        SetPlaying(true);
    }
    

    public void OnAnimatedEvent(bool isFire)
    {
        currNode.OnAnimatedEvent(isFire);
    }
    
    // controller에서 한번 거칠 필요 있을까 의문 필요
    public void OnDetected(EnemyDetectHandler.DetectType detectType, string value)
    {
        currNode.OnPhysicsDetected(detectType, value);
    }
    
    public void OnAgentDetected(EnemyAgent.Status status)
    {
        currNode.OnAgentDetected(status);
    }
    

    public void Define(Node newNode)    
    {
        currNode = new RootNode(newNode);
        SetPlaying(true);
    }

    public void Connect(Node node)
    {
        node.SetController(controller);
        if (node.children.Count > 0) { node.children.ForEach(Connect); } // method group 기능
    }

    public void Notify() // 특정 노드로 이동 기능 구현 필요
    {
        SetPlaying(false);
        
        currNode.SetController(controller);
        SetPlaying(true);
    }
    
}