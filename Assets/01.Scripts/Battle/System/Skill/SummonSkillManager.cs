using System.Collections.Generic;
using UnityEngine;

public class SummonSkillManager
{
    public enum Skill { DashAttack, ComboDashAttack, Agis, Heal, ArcherArrow, MudWave, MudEye, MoonFlyingSummonAttack }
    public enum Animation { AgisSpawn }
    
    public enum EnemySkill {}
    
    // 여기서 enemy도 연결해주면 되지 않을까?
    public static Dictionary<int, (Enemies.Enemy, Node)> skills { get; private set; } = new()
    {
        // notice: 노드 내부의 많은 속성을 외부 파라미터로 주입할 순 있지만 너무 세분화되어 관리 단위 짜기 어려움
        { (int)Skill.DashAttack, (Enemies.Enemy.NightBone, new SequenceNode(new InitNode(new Vector2(1.2f, 1.2f)), new DashAttack(), new CancelAttached(), new Explosion())) },
        { (int)Skill.ComboDashAttack, (Enemies.Enemy.SwordShadow, new SequenceNode(new InitNode(new Vector2(1.8f, 1.8f)), new ComboDashAttack())) },
        // bug: 사이즈 지정하기 전에 이미 생성되어버려서 큰 대상이 나타나는 문제 발생
        { (int)Skill.Agis, (Enemies.Enemy.Agis, new SequenceNode(new InitNode(new Vector2(0.4f, 0.4f)), new AgisSpreadShot())) },
        { (int)Skill.Heal, (Enemies.Enemy.Wizard, new SkillHealNode()) },
        { (int)Skill.ArcherArrow, (Enemies.Enemy.Archer, new RangeMultiAttackNode()) },
        { (int)Skill.MudWave, (Enemies.Enemy.MudHand, new SequenceNode(new MudCastingNode(), new MudAttackNode())) },
        { (int)Skill.MudEye, (Enemies.Enemy.MudEye, new RangeMultiAttackNode()) },
        { (int)Skill.MoonFlyingSummonAttack, (Enemies.Enemy.MoonStone, new MoonFlyingSummonAttack()) }
        
        // 사방으로 복제하기(적이 사용하려는 목적으로 이용)
        // 직접적으로 전달해줄 수 없으므로 내부 변수 등의 이용이 필요
    };

    public static Dictionary<Skill, (Enemies.Enemy, Node)> enemySkills { get; private set; } = new() { };

    public static Dictionary<int, (Enemies.Enemy, Node)> Animations { get; private set; } = new()
    {
        { (int)Animation.AgisSpawn, (Enemies.Enemy.Agis, new SequenceNode(new IdleNode(1), new Animation1())) },
    };
}