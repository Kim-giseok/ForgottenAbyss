
// 시퀀스를 싱글 노드를 기준으로 해보기

using System.Collections.Generic;
using UnityEngine;

public class RootNode : Node
{
    public RootNode(Node node)
    {
        node.SetParent(this);
        children.Add(node);
    }

    // 업데이트에서 할 경우 문제가 생길 수 있음
    public override void Update()
    {
        machine.SetCurrentNode(children[0]);

    }

    // 어떤 상태가 들어오든 다시 시작
    public override void GetStatus(Status newStatus, Node caller)
    {
        machine.OnLooped?.Invoke(); // check: 삭제되도 계속 진행되는 지 체크
        machine.SetCurrentNode(children[0]);
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
        machine.SetCurrentNode(children[0]);
    }

    public override void GetStatus(Status newStatus , Node caller)
    {

        if (newStatus == Status.Fail)
        {
            SetStatus(Status.Fail);
            return;
        }
        
        // bug: 복사본이 있다면 다음 인덱스로 인식하지 못하는 문제 발생
        int currIndex = children.IndexOf(caller);
        if (currIndex < children.Count - 1)
        {
            machine.SetCurrentNode(children[currIndex + 1]);
            return;
        }

        SetStatus(Status.Success);
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
        machine.SetCurrentNode(children[0]);
    }
    
    public override void GetStatus(Status newStatus, Node caller)
    {

        if (newStatus == Status.Fail)
        {
            int currIndex = children.IndexOf(caller);
            if (currIndex < children.Count - 1)
            {
                machine.SetCurrentNode(children[currIndex + 1]);
                return;
            }
            
            SetStatus(Status.Fail);
            return;
        }
        
        SetStatus(Status.Success);
    }
}


// 공통 노드로 가야할 듯
public class RandomNode : Node
{
    private List<float> percentage = new();
    public RandomNode(List<(float, Node)> children)
    {
        
        foreach ((float percent, Node node) in children)
        {
            percentage.Add(percent);
            node.SetParent(this);
            this.children.Add(node);
        }
    }

    public override void Start()
    {
        var random = Random.Range(0f, 1f);
        var selected = percentage.Find(percent => percent >= random);
        
        machine.SetCurrentNode(children[percentage.IndexOf(selected)]);
    }

    public override void GetStatus(Status newStatus, Node caller)
    {
        SetStatus(newStatus);
    }
}

// 추후 데코 노드 구현 필요 - 시퀀스 안에서 또 병렬 노드라면 문제 발생
// public class ParallelNode : Node
// {
//     public ParallelNode(params Node[] nodes)
//     {
//         foreach(Node child in nodes)
//         {
//             child.SetParent(this);
//             children.Add(child);
//         }
//     }
//     
//     public override void Start()
//     {
//         machine.SetCurrentNode(children[0]);
//     }
//
//     // notice: 복수 실행 자체는 BTMachine에서 처리하며 조건 감지만 이곳에서 처리
//     public override void GetStatus(Status newStatus, Node caller)
//     {
//         SetStatus(newStatus);
//     }
// }