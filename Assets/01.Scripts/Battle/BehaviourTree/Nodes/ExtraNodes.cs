using UnityEngine;

public class SetSizeNode: Node
{
    // public SetSizeNode(Vector2 size) { }
    public override void Start()
    {
        controller.transform.localScale = new Vector3(0.4f, 0.4f, 0);
        SetStatus(Status.Success);
    }
}