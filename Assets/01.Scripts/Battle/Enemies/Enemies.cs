using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Enemies
{
    public enum Enemy { Agis, Archer, Ghost, Gunner, NightBone, SwordShadow, Wizard }
    public static Node Get(Enemy enemy) => behaviour[enemy];

    private static Dictionary<Enemy, Node> behaviour = new()
    {
        {
            Enemy.NightBone,
            new SelectorNode(
                new SequenceNode(new HitNode()), 
                new SequenceNode(new TracingNode(), new MeleeAttack(), new IdleNode(0.5f)), 
                new SequenceNode(new IdleNode(1), new PatrolMove(1)))
        },
        {
            Enemy.Gunner,
            new SelectorNode(
                new SequenceNode(new HitNode()), 
                new SequenceNode(new TracingNode(), new RangeAttackNode()), 
                new SequenceNode(new IdleNode(1), new PatrolMove(1)))
        }
    };
}