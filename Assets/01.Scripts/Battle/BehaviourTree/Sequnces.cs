using System.Collections.Generic;

public class RootNode : Node
{
    private Node child;

    public RootNode(Node node)
    {
        child = node;
    }

    public override void Update()
    {
        btMachine.SetNode(child);
    }

    // 어떤 상태가 들어오든 다시 시작
    public override void GetStatus(Status newStatus)
    {
        btMachine.SetNode(child);    
    }
}

public class Sequence : Node
{
    private List<Node> children;
    public Sequence(params Node[] nodes)
    {
        children.AddRange(nodes);
    }

    public override void GetStatus(Status newStatus)
    {
        if (newStatus == Status.Fail)
        {
            parent.SetStatus(Status.Fail);
            return;
        }
        
        int currIndex = children.IndexOf(btMachine.currNode);
        if (currIndex < children.Count - 1)
        {
            btMachine.SetNode(children[currIndex + 1]);
            return;
        }
        
        parent.SetStatus(Status.Success);
        return;
    }
}

public class Selector : Node
{
    private List<Node> children;
    
    public Selector(params Node[] nodes)
    {
        children.AddRange(nodes);
    }
    
    public override void GetStatus(Status newStatus)
    {
        if (newStatus == Status.Fail)
        {
            int currIndex = children.IndexOf(btMachine.currNode);
            if (currIndex < children.Count - 1)
            {
                btMachine.SetNode(children[currIndex + 1]);
                return;
            }
            
                 
            parent.SetStatus(Status.Fail);
            return;
        }
    }
}