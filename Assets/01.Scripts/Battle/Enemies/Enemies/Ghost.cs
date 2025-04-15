public class GhostSummoningNode : Node
{
    public override void Start()
    {
        controller.animationHandler.Play("Summoning");
        ProjectileManager.Instance.CreateProjectile(controller.transform, 10f);
    }
}