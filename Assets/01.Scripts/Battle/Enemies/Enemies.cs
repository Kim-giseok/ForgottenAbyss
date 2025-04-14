using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Enemies
{
    public enum Enemy { NightBone }
    public static Node Get(Enemy enemy) => behaviour[enemy];

    private static Dictionary<Enemy, Node> behaviour = new()
    {
        {
            Enemy.NightBone, new SelectorNode(
                new SequenceNode(new HitNode()), new SequenceNode(new TracingNode(), new MeleeAttack()), 
                new SequenceNode(new IdleNode(1), new PatrolMove(1)))
        }
    };
}