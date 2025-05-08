using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BTMachine
{
    // 일반 BT는 컨트롤러를 주입 받는다. 프로젝 타일 역시 주입을 받아서 노드를 그리게 한다.
    private EnemyBaseController controller;
    public BTContext context = new();

    public bool isIgnoreRefresh = false;
    public bool isRefreshRequested = false;
    
    public Action OnLooped;
    
    public bool isPlaying { get; private set; } = false;
    public float currTime = 0;

    public Node rootNode { get; private set; }
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
        
        currNode = node;
        currNode.SetController(controller);
        
        currTime = 0;
        currNode.Start();
        
        SetPlaying(true);
    }
    

    public void OnAnimatedEvent(bool isFire)
    {
        currNode?.OnAnimatedEvent(isFire);
    }
    
    // 현재 노드가 없는 경우 문제 발생 
    // controller에서 한번 거칠 필요 있을까 의문 필요
    
    public void OnAgentDetected(EnemyAgent.Status status)
    {
        currNode?.OnAgentDetected(status);
    }
    

    public void Define(Node newNode)    
    {
        rootNode = new RootNode(newNode);
    }

    public void Start()
    {
        SetCurrentNode(rootNode);
    }

    public void Connect(Node node)
    {
        node.SetController(controller);
        if (node.children.Count > 0) { node.children.ForEach(Connect); } // method group 기능
    }

    public void Notify() // 특정 노드로 이동 기능 구현 필요
    {
        SetCurrentNode(rootNode);
    }
    
}