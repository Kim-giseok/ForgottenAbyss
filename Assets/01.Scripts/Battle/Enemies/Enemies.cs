using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemies
{
    // notice: enum을 추가하면 한칸씩 밀리는 현상 발생
    public enum Enemy { Test, Agis, Archer, Ghost, GhostChild, Gunner, NightBone, SwordShadow, Wizard, MudEye, MudHand, Bringer, MoonStone }
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
              new SequenceNode(new BlackHoleNode(), new AgisTriangleNode(), new MoveNode(3), new MoveNode(-3))
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
                new SequenceNode(new HitNode(), new DieNode()),
                new SequenceNode(new IdleNode(1), new PatrolMove(1))
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
                    (0.9f, new SequenceNode( new GunnerRangeAttack(), new IdleNode(0.05f), new GunnerRangeAttack())),
                    (1f, new GunnerRangeAttack())
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
                    new SequenceNode( new StopNode(), new IdleNode(1.6f), new WizadRecursiveNode(), new IdleNode(1.6f)),
                    new SequenceNode(new IdleNode(1.6f), new HealNode())
                )
        },
        {
            Enemy.MudEye,
            new SelectorNode(
                new SequenceNode(new HitNode(), new DieNode()),
                // new SequenceNode(new MudWarpNode(), new MudWarpNode(), new MudWarpNode())
                new SequenceNode(new IdleNode(1), new MudSpawnNode(), new MudWarpNode())
                )
        },
        {
            Enemy.MudHand,
            new SelectorNode(
                new SequenceNode(new HitNode(), new DieNode()),
                new SequenceNode(new MudAggroNode(), new MudCastingNode(), new MudAttackNode(), new MudIdleNode())
            )
        },
        {
            Enemy.Bringer,
            new SelectorNode(
                new SequenceNode(new HitNode(), new DieNode()),
                new SequenceNode(new TracingNode(), new StopNode(), new BringerAttackNode(), new IdleNode(1f)),
                new SequenceNode(new IdleNode(1), new PatrolMove(1))
                )
        },
        {
            Enemy.MoonStone,
            new SelectorNode(
                new SequenceNode(new HitNode(), new DieNode()),
                // new SequenceNode(new TracingNode(), new StopNode(), new BringerAttackNode(), new IdleNode(1f)),
                // new SequenceNode(new MoonCopy1(), new MoonAttack1(), new MoonWarp(), new MoonWarp())
                new SequenceNode(
                    new MoonAttack1(), new MoonStoneWalk(), new MoonCopyRain3(), new MoonFlyingMode(),  new MoonFlyingAttack(), new MoonFlyingEndNode(), 
                    new MoonWarp(), new MoonWarp(), new MoonWarp(), new MoonWarp(), new MoonCopy1(), new MoonAttack1(), new MoonCopy2()
                    )
                )
        }
    };
}