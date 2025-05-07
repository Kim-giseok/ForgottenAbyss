using System;
using System.Collections.Generic;

// SO로 관리되도록 한다.
public class EnemySkillCommand
{
    public string animName;

    // currTime 비용이 크므로 Time.time을 통해 plan을 세우기 전에 남은 시간을 찾도록 처리 
    public float lastEndTime; 
    public float cooldown;
    
    public float cost; // 마나비용
    public float castingTime;
    public float damage; // 혹은 계수로 관리
    
    public float afterCastingTime; // 스킬 사용 이후 지연 시간

    public float effect;
    public float range; // 사거리

    public enum AttackType { Base, OneShot, Dash }
    public AttackType attackType;
    
    public enum EndType {}

    public List<Node> SkillNode;

    // 앞에 condition부터 시작해서 캐스팅과 애프터 캐스팅까지 필요
    public List<Node> ResultNode;

    public Action action;
    public bool isLookTarget;

    public Node Get()
    {
        return new SequenceNode();
    }
}