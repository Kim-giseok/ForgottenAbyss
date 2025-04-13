using UnityEngine;

public class NormalModeNode : Node
{
    public override void Update()
    {
    }

    public override void OnAgentDetected(EnemyAgent.Status status)
    {
        if (status != EnemyAgent.Status.None)
        {
            SetStatus(Status.Fail);
        }
    }
}

public class CombatModeNode : Node
{
    public override void OnAgentDetected(EnemyAgent.Status status)
    {
        if (status == EnemyAgent.Status.None)
        {
            Debug.Log("battle end");
            SetStatus(Status.Fail);
        }
    }
}