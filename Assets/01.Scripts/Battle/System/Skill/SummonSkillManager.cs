using System.Collections.Generic;
using Summon;

public class SummonSkillManager
{
    public enum Skill { DashAttack, ComboDashAttack }
    public enum EnemySkill {}
    
    // 여기서 enemy도 연결해주면 되지 않을까?
    public static Dictionary<Skill, (Enemies.Enemy, Node)> skills { get; private set; } = new()
    {
        // notice: 노드 내부의 많은 속성을 외부 파라미터로 주입할 순 있지만 너무 세분화되어 관리 단위 짜기 어려움
        { Skill.DashAttack, (Enemies.Enemy.NightBone, new SequenceNode(new DashAttack(), new CancelAttached(), new Explosion())) }
    };

    public static Dictionary<Skill, (Enemies.Enemy, Node)> enemySkills { get; private set; } = new();
}