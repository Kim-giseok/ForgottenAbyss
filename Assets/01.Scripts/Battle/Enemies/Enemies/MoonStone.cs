using UnityEngine;

public class MoonStoneWalk : Node
{
    public override void Start()
    {
        controller.animnHandler.Play("Walk");
    }

    public override void Update()
    {
        if(currTime > 2f) { SetStatus(Status.Success); return; }

        Vector2 direction = controller.rigidbody.velocity;
        direction.x = 0.4f;
        controller.rigidbody.velocity = direction;
    }
}

public class MoonWarp : Node
{
    public override void Start()
    {   
        controller.animnHandler.Play("Warp");

        // 랜덤이 안됨
        var selected = NavSurface.Instance.platforms[Random.Range(0, 4)].centerCell.WorldPos;
        // 타일 사이즈 절반 차감
        selected.y += controller.collider.bounds.size.y - 0.5f;
       
        controller.transform.position = selected;
    }

    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (!animInfo.IsName("Warp")) return;
        
        if (status == AnimationStatus.End)
        {
            SetStatus(Status.Success);
        }
    }
}