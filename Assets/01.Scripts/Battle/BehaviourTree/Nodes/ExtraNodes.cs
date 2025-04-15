using UnityEngine;

public class SetSizeNode: Node
{
    public override void Start()
    {
        controller.transform.localScale = new Vector3(2, 2, 0);
        SetStatus(Status.Success);
    }
}