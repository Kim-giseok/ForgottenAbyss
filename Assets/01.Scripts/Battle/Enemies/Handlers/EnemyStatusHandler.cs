public class EnemyStatusHandler
{
    public bool isIgnoreHitAction = false; // 원거리 친구만 false
    
    public bool isHit = false;
    public bool isDefense = false;
    
    public bool isFainted = false; // notice: 기절 기능 - 난이도
    public int stamina = 3; // 0일 경우 기절
    public int faintedDuration = 3;

    public bool isCombat = false; // 한번 전투 중이면 지속 체크
}