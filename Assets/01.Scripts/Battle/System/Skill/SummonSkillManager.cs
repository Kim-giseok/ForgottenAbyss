using System.Collections.Generic;
using Summon;
using UnityEngine;

public class SummonSkillManager
{
    public enum Skill { DashAttack, ComboDashAttack, BossSkill, BossSkill2, BossSkill3 }
    public enum EnemySkill {}
    
    // 여기서 enemy도 연결해주면 되지 않을까?
    public static Dictionary<Skill, (Enemies.Enemy, Node)> skills { get; private set; } = new()
    {
        // notice: 노드 내부의 많은 속성을 외부 파라미터로 주입할 순 있지만 너무 세분화되어 관리 단위 짜기 어려움
        { Skill.DashAttack, (Enemies.Enemy.NightBone, new SequenceNode(new InitNode(new Vector2(1.2f, 1.2f)), new DashAttack(), new CancelAttached(), new Explosion())) },
        
        // bug: 사이즈 지정하기 전에 이미 생성되어버려서 큰 대상이 나타나는 문제 발생
        { Skill.BossSkill, (Enemies.Enemy.Agis, new SequenceNode(new InitNode(new Vector2(0.4f, 0.4f)), new SpreadShotNode(Vector2.up))) },
        
        // 사방으로 복제하기(적이 사용하려는 목적으로 이용)
        // 직접적으로 전달해줄 수 없으므로 내부 변수 등의 이용이 필요
        { Skill.BossSkill2, (Enemies.Enemy.Agis, new SequenceNode(new InitNode(new Vector2(0.4f, 0.4f)), new SpreadShotNode(Vector2.left))) },
        { Skill.BossSkill3, (Enemies.Enemy.Agis, new SequenceNode(new InitNode(new Vector2(0.4f, 0.4f)), new SpreadShotNode(Vector2.right))) }

    };

    public static Dictionary<Skill, (Enemies.Enemy, Node)> enemySkills { get; private set; } = new()
    {
       };
}