using System.Collections.Generic;
using UnityEngine;

public class RootNode : Node
{
    private Node child;

    public RootNode(Node node)
    {
        node.SetParent(this);
        child = node;
    }

    public override void Update()
    {
        btMachine.SetNode(child);
    }

    // 어떤 상태가 들어오든 다시 시작
    public override void GetStatus(Status newStatus, Node caller)
    {
        Debug.Log("restart");
        btMachine.SetNode(child);    
    }
}

public class Sequence : Node
{
    private List<Node> children = new();
    public Sequence(params Node[] nodes)
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
            Debug.Log("fail from sequence");
            SetStatus(Status.Fail);
            return;
        }
        
        int currIndex = children.IndexOf(caller);
        if (currIndex < children.Count - 1)
        {
            btMachine.SetNode(children[currIndex + 1]);
            return;
        }
        
        Debug.Log("success from here");
        SetStatus(Status.Success);
        return;
    }
}

public class Selector : Node
{
    private List<Node> children = new();
    
    public Selector(params Node[] nodes)
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
            
            Debug.Log(currIndex);
            if (currIndex < children.Count - 1)
            {
                btMachine.SetNode(children[currIndex + 1]);
                Debug.Log(children[currIndex + 1]);
                return;
            }
            
                 
            SetStatus(Status.Fail);
            return;
        }
        
        SetStatus(Status.Success);
    }
}