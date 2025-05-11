using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillSO: ScriptableObject
{
    public string animName;

    // currTime 비용이 크므로 Time.time을 통해 plan을 세우기 전에 남은 시간을 찾도록 처리 
    public float lastEndTime; 
    public float cooldown;
    
    public float cost; // 마나비용
    public float castingTime;

    public float afterCastingTime; // 스킬 사용 이후 지연 시간
    public bool isLookTarget;

    public float effect;
    public float range; // 사거리

    public enum AttackType { Base, OneShot, Dash }
    public AttackType attackType;
    
    public enum EndType {}

    public List<Node> SkillNode;

    // 앞에 condition부터 시작해서 캐스팅과 애프터 캐스팅까지 필요
    public List<Node> ResultNode;

    public System.Action action;

    public Node Get()
    {
        return new Sequence();
    }
}