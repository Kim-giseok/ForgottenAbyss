
// animHandler가 액션 이후에도 살아있기 때문에 NPC로 두어 비용 발생하는 부분 방지하기

using UnityEngine;

public class ActorController: EnemyBaseController
{
    public bool IsEnd { get; private set; } = true;
    
    public void Define(Node node)
    {
        IsEnd = false;
        // 기본 노드 안에 waitNode하나를 둬서 다음 행동이 발생하기 전까지 기다리게 하기
        // 한텀 돌면 삭제가 아니라 잠시 정지
        machine.OnLooped += () =>
        {
            machine.SetPlaying(false);
            IsEnd = true;
        };
        
        machine.Define(node);
        machine.Start();
    }
}