using UnityEngine;

public class Animation1 : Node
{
    public override void Start()
    {
        Debug.Log("animated");
        SetStatus(Status.Success);
    }
}