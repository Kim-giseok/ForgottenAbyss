using BT;
using UnityEngine;
// 특정 위치만큼 이동하거나 하는 방식도 가능해야함
// 특정 시간 또는 애니메이션의 종료에 의존됨

public class MoveNode : Node
{
    private readonly MoveCommand command;
    public MoveNode(MoveCommand command) => this.command = command;

    public override void Start()
    {
        if (command.isInvincible)
        {
            (controller as EnemyController)!.isInvincible = true;
        }
    }

    public override void Update()
    {
        // 추적 형태인 경우
        if (command.isTrackingTarget)
        {
            controller.Rigidbody.velocity = new Vector2(controller.agent.GetDirection().x * controller.agent.tracingSpeed, controller.Rigidbody.velocity.y);
            return;
        }

        if (command.direction != Vector2.zero)
        {
            controller.Rigidbody.velocity = command.direction * controller.agent.tracingSpeed;
            return;
        }
    }

    public override void End()
    {
        if (command.isInvincible)
        {
            (controller as EnemyController)!.isInvincible = false;
        }
    }
}


public class MovePlatformNode : Node
{
    public override void Start()
    {
        if (NavSurface.Instance.GetPlatformId(controller.agent.target) == NavSurface.Instance.GetPlatformId(controller.gameObject))
        {
            // 플랫폼 이동과 추적 간의 순서는 좀 더 생각해보기
            SetStatus(Status.Success);
            return;
        }
        
        var targetPlatform = NavSurface.Instance.platforms.Find(platform => platform.id == NavSurface.Instance.GetPlatformId(controller.agent.target));
        Vector2 destination = targetPlatform.centerCell.WorldPos;
        context.Set("destination", new Vector3(destination.x, destination.y, 0));
        
        controller.Collider.isTrigger = true;
        controller.Rigidbody.gravityScale = 0;
        // controller.rigidbody.isKinematic = true;
    }

    public override void Update()
    {
        Vector3 destination = context.Get<Vector3>("destination") + Vector3.up;
        // 박싱으로 인한 성능 문제 예상해보기
        Vector3 direction = (destination - controller.transform.position).normalized;
        controller.transform.position += direction * (Time.deltaTime * 6f);  // 이동

        // 목표에 거의 도달하면 완료
        if (Vector3.Distance(destination, controller.transform.position) < 0.5f)
        {
            // 중앙까지는 잘 도착햇지만 다시 플레이어 추적을 해야해서 에러남
            Debug.Log(3);
            SetStatus(Status.Success);
        }
    }

    public override void End()
    {
        controller.Collider.isTrigger = false;
        // controller.rigidbody.isKinematic = false;
        controller.Rigidbody.gravityScale = 2;
    }
}