public class GhostSummoningNode : Node
{
    public override void Start()
    {
        controller.animnHandler.Play("Summoning");
        BoltManager.Instance.CreateProjectile(controller.transform, 10f);
    }
}