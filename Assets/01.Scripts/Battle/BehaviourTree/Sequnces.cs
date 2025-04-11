using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class RootNode : Node
{
    private Action OnLooped;
    public RootNode(Node node)
    {
        node.SetParent(this);
        children.Add(node);
    }
    
    public RootNode WhenLooped(Action currEvent)
    {
        OnLooped += currEvent;
        return this;
    }

    public override void Update()
    {
        btMachine.SetNode(children[0]);
    }

    // 어떤 상태가 들어오든 다시 시작
    public override void GetStatus(Status newStatus, Node caller)
    {
        OnLooped?.Invoke();
        btMachine.SetNode(children[0]);    
    }
}

public class SequenceNode : Node
{
    public SequenceNode(params Node[] nodes)
    {
        foreach (Node child in nodes)
        {
            child.SetParent(this);
            children.Add(child);
        }
    }

    public override void Start()
    {
        btMachine.SetNode(children[0]);
    }

    public override void GetStatus(Status newStatus , Node caller)
    {
        if (newStatus == Status.Fail)
        {
            SetStatus(Status.Fail);
            return;
        }
        
        int currIndex = children.IndexOf(caller);
        if (currIndex < children.Count - 1)
        {
            btMachine.SetNode(children[currIndex + 1]);
            return;
        }
        
        SetStatus(Status.Success);
        return;
    }
}

public class SelectorNode : Node
{
    public SelectorNode(params Node[] nodes)
    {
        foreach(Node child in nodes)
        {
            child.SetParent(this);
            children.Add(child);
        }
    }
    
    public override void Start()
    {
        btMachine.SetNode(children[0]);
    }
    
    public override void GetStatus(Status newStatus, Node caller)
    {
        if (newStatus == Status.Fail)
        {
            int currIndex = children.IndexOf(caller);
            
            if (currIndex < children.Count - 1)
            {
                btMachine.SetNode(children[currIndex + 1]);
                return;
            }
            
                 
            SetStatus(Status.Fail);
            return;
        }
        
        SetStatus(Status.Success);
    }
}

public class ParallelNode : Node
{
    public ParallelNode(params Node[] nodes)
    {
        children.AddRange(nodes);
    }

    public override void Start()
    {
        // 복수로 등록
        // btMachine.SetNode(children);
    }

    // notice: 복수 실행 자체는 BTMachine에서 처리하며 조건 감지만 이곳에서 처리
    public override void GetStatus(Status newStatus, Node caller)
    {
        // notice: 하나의 노드에서 응답 받은 경우, 상위 노드에게 바로 전달
        SetStatus(newStatus);
    }
}

public class RandomNode : Node
{
    
}

// 어떻게 데코할 것인가?
public class DecoratorNode : Node
{
    public DecoratorNode(Node child)
    {
        children.Add(child);
    }
}