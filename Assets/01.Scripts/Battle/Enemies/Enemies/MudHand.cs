using UnityEngine;

public class MudHandIdleNode : Node
{
    public override void Update()
    {
        if (currTime > 3f)
        {
            SetStatus(Status.Success);
            return;
        }

        if (NavSurface.Instance.GetTarget(controller.gameObject) ==
            NavSurface.Instance.GetTarget(GameManager.Instance.player.gameObject))
        {
            Debug.Log("here");
        }
    }
}