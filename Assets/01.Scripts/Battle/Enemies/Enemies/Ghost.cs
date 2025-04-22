public class GhostSummoningNode : Node
{
    public override void Start()
    {
        controller.animnHandler.Play("Summoning");
        BoltsPool.Instance.Create(controller.transform, 10f);
    }
}