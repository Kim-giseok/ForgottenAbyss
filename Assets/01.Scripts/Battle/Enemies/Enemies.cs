using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemies
{
    // notice: enum을 추가하면 한칸씩 밀리는 현상 발생
    public enum Enemy { Test, Agis, Archer, Ghost, GhostChild, Gunner, NightBone, SwordShadow, Wizard, MudEye, MudChildHand }
    public static Node Get(Enemy enemy) => behaviour[enemy];

    private static Dictionary<Enemy, Node> behaviour = new()
    {
        {
            Enemy.Test,
            new SelectorNode(new IdleNode(1f))
        },
        {
          Enemy.Agis,
          new SelectorNode(
              new SequenceNode(new HitNode(), new SetZeroPosNode(), new DieNode()),
              new SequenceNode(new MoveNode(3), new MoveNode(-3))
              )
        },
        {
            // 거리에 길수록 아처에게 유리해지도록 처리
            // 너무 많이 다가오면 롤링
            Enemy.Archer,
            new SelectorNode(
                new SequenceNode(new HitNode(), new DieNode()),
                new SequenceNode(new TracingNode(), new StopNode(), new ChargingNode(1f), new RangeMultiAttackNode()),
                new SequenceNode(new IdleNode(1), new PatrolMove(1))
                )
        },
        {
            Enemy.Ghost,
            new SelectorNode(
                new SequenceNode(new HitNode(), new DieNode())
            )
        },
        {
            // 관통 + 튕기는 효과를 주로 다루는
            Enemy.Gunner,
            new SelectorNode(
                new SequenceNode(new HitNode(), new DieNode()), 
                new SequenceNode(new TracingNode(), 
                    new ChargingNode(1f),
                    new RandomNode(new()
                    {
                    //     // 스킬을 사용한 경우 마나와 쿨타임 정보 가지는 방식 필요
                    //     // 연속 공격이 왜 안됨(애님메이션 관련 문제)
                    (0.9f, new SequenceNode( RangeAttack.Piercing, new IdleNode(0.05f), RangeAttack.Piercing)),
                    (1f, RangeAttack.Piercing)
                    }),
                    new CoolTimeNode(0.5f)), 
                new SequenceNode(new IdleNode(1), new PatrolMove(1)))
        },
        {
            // 탱커 - 폭팔 발생 시 도주 필요
            Enemy.NightBone,
            new SelectorNode(
                new SequenceNode(new HitNode(), new DieNode()), 
                new SequenceNode(
                    new SequenceNode( new TracingNode(), new TracingNode()),
                    new SequenceNode(new StopNode(), new RandomNode(new()
                    {
                        (0.2f, new SequenceNode(new ChargingNode(2f), new ExplosionNode())),
                        (1f, new SequenceNode(new MeleeAttack(), new IdleNode(0.5f)))
                    }))), 
                new SequenceNode(new IdleNode(1), new PatrolMove(1)))
        },
        {
            // 소드맨 - 속도가 빨라서 원거리 공격이 파훼법(방어가 필요할 듯)
            Enemy.SwordShadow,
            new SelectorNode(
                new SequenceNode(new HitNode(), new DieNode()), 
                new SequenceNode(
                    new TracingNode(),
                    new SSDashAttack("Combo1"),  new RandomCoolTimeNode(),
                    new SSDashAttack("Combo2"), new RandomCoolTimeNode(),
                    new SSDashAttack("Combo3"), new RandomCoolTimeNode() 
                ),
                new SequenceNode(new IdleNode(1), new PatrolMove(1)))
        },
        {
            Enemy.Wizard,
            new SelectorNode(
                new SequenceNode(new HitNode(), new DieNode()), 
                    new SequenceNode(new TracingNode(), new StopNode(), new WizadRecursiveNode(), new IdleNode(3f)),
                    new SequenceNode(new IdleNode(3), new HealNode())
                )
        },
        {
            Enemy.MudEye,
            new SelectorNode(
                new SequenceNode(new HitNode(), new DieNode()),
                new SequenceNode(new IdleNode(1), new MudControlnode())
                )
        },
        {
            Enemy.MudChildHand,
            new SelectorNode(
                new SequenceNode(new HitNode(), new DieNode()),
                new SequenceNode(new MudHandIdleNode())
            )
        }
    };
}