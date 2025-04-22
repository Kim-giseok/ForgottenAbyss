using UnityEngine;

public class MudControlnode : Node
{
    public override void Update()
    {
        if (controller is not EnemyController eController) return;
        // Debug.Log(eController.children.Count);
        
        // eController.children[0].transform.localScale = Vector3.Lerp(eController.children[0].transform.localScale, Vector3.one * 2f, 0.1f);
    }
}