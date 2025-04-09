using System.Collections.Generic;
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
        btMachine.SetNode(children[0]);
    }

    // 어떤 상태가 들어오든 다시 시작
    public override void GetStatus(Status newStatus, Node caller)
    {
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
    }

    public override void Update()
    {
    }

    public override void End()
    {
    }
}


public class DecoratorNode : Node
{
    public DecoratorNode(Node child)
    {
        children.Add(child);
    }
}