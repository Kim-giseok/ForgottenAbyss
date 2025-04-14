using System.Collections.Generic;
using Summon;

public class SummonSkillManager
{
    public enum Skill { DashAttack }
    
    // 여기서 enemy도 연결해주면 되지 않을까?
    private static Dictionary<Skill, (Enemies.Enemy, Node)> skills = new()
    {
        { Skill.DashAttack, (Enemies.Enemy.NightBone, new DashAttack()) }
    };
    
    public static (Enemies.Enemy, Node) Get(Skill skill) => skills[skill];
}