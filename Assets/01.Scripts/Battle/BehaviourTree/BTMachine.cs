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
        
        Modify(node => node.Update());
    }

    public void SetPlaying(bool isPlaying)
    {
        this.isPlaying = isPlaying;
    }
    
        
    // ReSharper disable Unity.PerformanceAnalysis
    private void Modify(Action<Node> callback) // 비용 큰 점 고민 필요
    {
        // var snapshot = currNodes.ToList();
        currNodes.ForEach(node =>
        {
            node.SetController(controller);
            callback(node);
        });
    }

    // 변화 체크가 진행되지 않음
    public void Fetch()
    {
        SetPlaying(false);

        Modify(node => node.End()); // 없는 end도 결국 호출 발생
        
        currTime = 0;
        
        Modify(node => node.Start());

        SetPlaying(true);
    }

    public void OnAnimatedEvent(bool isFire)
    {
        Modify(node => node.OnAnimatedEvent(isFire));
    }
    
    // controller에서 한번 거칠 필요 있을까 의문 필요
    public void OnDetected(EnemyDetectHandler.DetectType detectType, string value)
    {
        Modify(node => node.OnPhysicsDetected(detectType, value));
    }
    
    public void OnAgentDetected(EnemyAgent.Status status)
    {
        Modify(node => node.OnAgentDetected(status));
    }
    

    public void Define(Node newNode)    
    {
        rootNode = new RootNode(newNode);
        currNodes.Clear();
        currNodes.Add(rootNode);
        
        Fetch();
    }

    public void Connect(Node node)
    {
        node.SetController(controller);
        if (node.children.Count > 0) { node.children.ForEach(Connect); } // method group 기능
    }

    public void Notify() // 특정 노드로 이동 기능 구현 필요
    {
        SetPlaying(false);
        
        currNodes.Clear();
        currNodes.Add(rootNode);
        Fetch();
    }
}