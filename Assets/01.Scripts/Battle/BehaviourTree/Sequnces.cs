
// 시퀀스를 싱글 노드를 기준으로 해보기

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
        machine.OnLooped?.Invoke();
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