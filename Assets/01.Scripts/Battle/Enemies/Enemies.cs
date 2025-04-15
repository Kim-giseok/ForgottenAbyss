using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Enemies
{
    public enum Enemy { Agis, Archer, Ghost, GhostChild, Gunner, NightBone, SwordShadow, Wizard }
    public static Node Get(Enemy enemy) => behaviour[enemy];

    private static Dictionary<Enemy, Node> behaviour = new()
    {
        {
          Enemy.Agis,
          new SelectorNode()
        },
        {
            Enemy.Ghost,
            new SelectorNode(
                new SequenceNode(new HitNode(), new DieNode())
            )
        },
        {
            Enemy.Gunner,
            new SelectorNode(
                new SequenceNode(new HitNode()), 
                new SequenceNode(new TracingNode(), new RangeAttackNode()), 
                new SequenceNode(new IdleNode(1), new PatrolMove(1)))
        },
        {
            // 탱커 - 폭팔 발생 시 도주 필요
            Enemy.NightBone,
            new SelectorNode(
                new SequenceNode(new HitNode(), new DieNode()), 
                new SequenceNode(new TracingNode(), new StopNode(), 
                    new RandomNode(new()
                    {
                        (0.3f, new SequenceNode(new ChargingNode(), new ExplosionNode())),
                        (1f, new SequenceNode(new MeleeAttack(), new IdleNode(0.5f)))
                    })), 
                new SequenceNode(new IdleNode(1), new PatrolMove(1)))
        },
        {
            // 소드맨 - 속도가 빨라서 원거리 공격이 파훼법
            Enemy.SwordShadow,
            new SelectorNode(
                new SequenceNode(new HitNode(), new DieNode()), 
                new SequenceNode(
                    new TracingNode(), new ComboAttackNode() 
                ),
                new SequenceNode(new IdleNode(1), new PatrolMove(1)))
        }
    };
}