public class EnemySkill
{
    // currTime 비용이 크므로 Time.time을 통해 plan을 세우기 전에 남은 시간을 찾도록 처리 
    public float lastEndTime; 
    public float cooldown;
    public float cost;

    public float effect;
    public float range; // 사거리
    
    // 노드는 공유하되 
    public static Node flowNode = new SequenceNode(); // 아예 이쪽에서 등록?
}