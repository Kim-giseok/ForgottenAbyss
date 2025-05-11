
// 시퀀스를 싱글 노드를 기준으로 해보기

using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace BT
{
 public class Root : Node
{
    public Root(Node node)
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
        // notice: destroy 프레임이 끝나기 전에 삭제되지 않늨 현상에 대한 대처 필요
        if (machine.OnLooped != null) return;
        machine.SetCurrentNode(children[0]);
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

// 문제 발생할 수 있음
// 액션 노드를 동시에 실행하는 용도로 사용 - 이벤트 인식이 불가능
public class ParallelNode : Node
{
    public ParallelNode(params Node[] nodes)
    {
        foreach (Node child in nodes)
        {
            child.SetParent(this);
            children.Add(child);
        }
    }

    public override void Start()
    {
        foreach (var child in children)
        {
            child.SetController(controller);
            child.Start();
        }
    }

    public override void Update()
    {
        foreach (var child in children)
        {
            child.SetController(controller);
            child.Update();
        }
    }

    public override void End()
    {
        foreach (var child in children)
        {
            child.SetController(controller);
            child.End();
        }
    }
}


// 컨디션 노드 추가
public class Condition : Node
{
    private readonly Func<EnemyBaseController, bool> callback;

    public Condition(Func<EnemyBaseController, bool> callback, Node child)
    {
        this.callback = callback;
        
        child.SetParent(this);
        children.Add(child);
    }

    public override void Start()
    {
        bool result = callback(controller);
        if (!result)
        {
            SetStatus(Status.Fail);
            return;
        }
        
        machine.SetCurrentNode(children[0]);
    }

    public override void GetStatus(Status newStatus, Node caller)
    {
        SetStatus(newStatus);
    }
}   
}