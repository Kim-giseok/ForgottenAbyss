using System.Collections.Generic;
using UnityEngine;

public class SummonSkillManager
{
    public enum Skill
    {
        DashAttack, ComboDashAttack, Agis, AgisRain, Heal, ArcherArrow, MudWave, MudEye, MoonFlyingSummonAttack, }
    
    public enum EnemySkill {}
    
    // 여기서 enemy도 연결해주면 되지 않을까?
    public static Dictionary<int, (Enemy, Node)> skills { get; private set; } = new()
    {
        // notice: 노드 내부의 많은 속성을 외부 파라미터로 주입할 순 있지만 너무 세분화되어 관리 단위 짜기 어려움
        { (int)Skill.DashAttack, (Enemy.NightBone, new SequenceNode(new DashAttack(), new CancelAttached(), new Explosion())) },
        { (int)Skill.ComboDashAttack, (Enemy.SwordShadow, new SequenceNode(new InitNode(new Vector2(1.8f, 1.8f)), new ComboDashAttack())) },
        // bug: 사이즈 지정하기 전에 이미 생성되어버려서 큰 대상이 나타나는 문제 발생
        { (int)Skill.Agis, (Enemy.Agis, new SequenceNode(new AgisSpreadShot())) },
        { (int)Skill.AgisRain, (Enemy.Agis, new SequenceNode(new InitNode(new Vector2(0.4f, 0.4f)), new AgisRainNode())) },
        { (int)Skill.Heal, (Enemy.Wizard, new SkillHealNode()) },
        { (int)Skill.ArcherArrow, (Enemy.Archer, new RangeMultiAttackNode()) },
        { (int)Skill.MudWave, (Enemy.MudHand, new SequenceNode(new MudCastingNode(), new MudAttackNode())) },
        { (int)Skill.MudEye, (Enemy.MudEye, new MudEyeSpell()) },
        { (int)Skill.MoonFlyingSummonAttack, (Enemy.MoonStone, new MoonFlyingSummonAttack()) }
        
        // 사방으로 복제하기(적이 사용하려는 목적으로 이용)
        // 직접적으로 전달해줄 수 없으므로 내부 변수 등의 이용이 필요
    };

    public static Dictionary<Skill, (Enemy, Node)> enemySkills { get; private set; } = new() { };

    public static Dictionary<int, (Enemy, Node)> Animations { get; private set; } = new()
    {
    };
}