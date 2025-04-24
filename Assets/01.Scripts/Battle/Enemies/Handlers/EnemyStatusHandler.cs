using UnityEngine;

// 디폴트는 동일하게 가져가지만 몬스터마다 다르게
public class EnemyStatusHandler
{
    public bool isIgnoreHitAction = false; // 원거리 친구만 false
    public Vector3 startPosition;
    
    public bool isHit = false;
    public bool isDefense = false;
    
    public bool isFainted = false; // notice: 기절 기능 - 난이도
    public int stamina = 3; // 스테미나의 경우 다른 목적으로 사용(이동 정도라던지)
    public int stunAccumulation;

    public int bullete; // 총알의 갯수
    
    
    public int faintedDuration = 3;

    public bool isCombat = false; // 한번 전투 중이면 지속 체크
    
    public Vector2 castingDirection; // 스킬을 발사하는 방향
    public float castingDegree;
}