using System;
using UnityEngine;

public class RootNode : Node
{
    public RootNode(Node node)
    {
        node.SetParent(this);
        children.Add(node);
    }

    public override void Update()
    {
        machine.SetNode(children[0]);
    }

    // 어떤 상태가 들어오든 다시 시작
    public override void GetStatus(Status newStatus, Node caller)
    {
        machine.OnLooped?.Invoke();
        machine.SetNode(children[0]);    
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
        machine.SetNode(children[0]);
    }

    public override void GetStatus(Status newStatus , Node caller)
    {
        if (newStatus == Status.Fail)
        {
            SetStatus(Status.Fail);
            return;
        }
        
        // 복사본이 있다면 다음 인덱스로 인식하지 못하는 문제 발생
        int currIndex = children.IndexOf(caller);
        
        if (currIndex < children.Count - 1)
        {
            machine.SetNode(children[currIndex + 1]);
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
        machine.SetNode(children[0]);
    }
    
    public override void GetStatus(Status newStatus, Node caller)
    {
        if (newStatus == Status.Fail)
        {
            int currIndex = children.IndexOf(caller);
            
            if (currIndex < children.Count - 1)
            {
                machine.SetNode(children[currIndex + 1]); // notice: BtMachine 매번 호출하여 코드 길어짐
                return;
            }
            
                 
            SetStatus(Status.Fail);
            return;
        }
        
        SetStatus(Status.Success);
    }
}

// 추후 데코 노드 구현 필요
public class ParallelNode : Node
{
    public ParallelNode(params Node[] nodes)
    {
        foreach(Node child in nodes)
        {
            child.SetParent(this);
            children.Add(child);
        }
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    public override void Start()
    {
        machine.SetNode(children.ToArray());
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