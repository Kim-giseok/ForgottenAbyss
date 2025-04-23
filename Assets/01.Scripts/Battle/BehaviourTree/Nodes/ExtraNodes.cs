using UnityEngine;

public class InitNode: Node
{
    private readonly Vector2 size;
    
    public InitNode(Vector2 size) => this.size = size;
    public override void Start()
    {
        controller.transform.localScale = size;
        controller.renderer.enabled = true;
        SetStatus(Status.Success);
    }
}