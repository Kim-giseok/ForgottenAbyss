// node에 index 
public class Sequence : Node
{
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
        children[0].SetParent(this);
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
            children[currIndex + 1].SetParent(this);
            machine.SetCurrentNode(children[currIndex + 1]);
            return;
        }

        SetStatus(Status.Success);
    }
}