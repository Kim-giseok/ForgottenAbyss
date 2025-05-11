namespace BT
{
    public class Selector : Node
    {
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
}
