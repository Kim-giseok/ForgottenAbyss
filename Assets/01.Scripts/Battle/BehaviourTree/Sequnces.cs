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

        machine.SetPlaying(false);

        machine.currNodes.Clear();
        machine.currNodes.Add(children[0]);
        
        machine.Fetch();
    }

    // 어떤 상태가 들어오든 다시 시작
    public override void GetStatus(Status newStatus, Node caller)
    {
        machine.SetPlaying(false);
        
        machine.OnLooped?.Invoke();
        
        machine.currNodes.Clear();
        machine.currNodes.Add(children[0]);
        
        machine.Fetch();
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
        
        machine.SetPlaying(false);

        machine.currNodes.Remove(this);
        machine.currNodes.Add(children[0]);
        
        machine.Fetch();
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
            machine.SetPlaying(false);

            machine.currNodes.Remove(children[currIndex]);
            machine.currNodes.Add(children[currIndex + 1]);
            
            machine.Fetch();
            
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
        machine.SetPlaying(false);

        machine.currNodes.Remove(this);
        machine.currNodes.Add(children[0]);
        
        machine.Fetch();
    }
    
    public override void GetStatus(Status newStatus, Node caller)
    {
        if (newStatus == Status.Fail)
        {
            int currIndex = children.IndexOf(caller);
            
            if (currIndex < children.Count - 1)
            {
                machine.SetPlaying(false);

                machine.currNodes.Remove(children[currIndex]);
                machine.currNodes.Add(children[currIndex + 1]);
                
                machine.Fetch();

                return;
            }
            
                 
            SetStatus(Status.Fail);
            return;
        }
        
        SetStatus(Status.Success);
    }
}

// 추후 데코 노드 구현 필요 - 시퀀스 안에서 또 병렬 노드라면 문제 발생
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
    
    public override void Start()
    {
        machine.SetPlaying(false);
        machine.currNodes.AddRange(children.ToArray());
        machine.Fetch();
    }

    // notice: 복수 실행 자체는 BTMachine에서 처리하며 조건 감지만 이곳에서 처리
    public override void GetStatus(Status newStatus, Node caller)
    {
        machine.SetPlaying(false);
        foreach (Node child in children) { machine.currNodes.Remove(child); }
        machine.Fetch();

        // notice: 하나의 노드에서 응답 받은 경우, 상위 노드에게 바로 전달
        SetStatus(newStatus);
    }
}