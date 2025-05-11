using System;
using System.Collections.Generic;
using System.Linq;
using BT;
using UnityEngine;

public class EnemiesBT
{
    // notice: enum을 추가하면 한칸씩 밀리는 현상 발생
    public enum Enemy { Agis, Archer, Ghost, GhostChild, Gunner, NightBone, SwordShadow, Wizard, MudEye, MudHand, Bringer, MoonStone }
    public static Node Get(string enemy)
    {
        Debug.LogError(behaviour[enemy]);
        return behaviour[enemy];
    }

    private static Dictionary<string, Node> behaviour = new()
    {
        // 쿨타임 개념을 가지거나, 시간을 자체적으로 가지게 해서 공격을 발사하는
        // {
        //   Enemy.Agis,
        //   new Selector(
        //       new Sequence(Nodes.Hit, new SetZeroPosNode(), Nodes.Die),
        //       // new SequenceNode(new BlackHoleNode(), new AgisTriangleNode(), new AgisMoveNode(3), new AgisMoveNode(-3))
        //       new Sequence(new AgisMoveNode())
        //       )
        // },
        // {
        //     // 거리에 길수록 아처에게 유리해지도록 처리
        //     // 너무 많이 다가오면 롤링
        //     Enemy.Archer,
        //     new Selector(
        //         new Sequence(Nodes.Hit, Nodes.Die),
        //         new Sequence(new TracingNode(), 
        //             new Wait().Animation("Casting").Stop(true).Duration(2f).Build()
        //             // new RangeMultiAttackNode()
        //             ),
        //         new Sequence(new IdleNode(1), new PatrolMove(1))
        //         )
        // },
        // {
        //     Enemy.Ghost,
        //     new Selector(
        //         new Sequence(Nodes.Hit, Nodes.Die),
        //         new Sequence(new IdleNode(1), new PatrolMove(1))
        //     )
        // },
        // {
        //     // 관통 + 튕기는 효과를 주로 다루는
        //     Enemy.Gunner,
        //     new Selector(
        //         new Sequence(Nodes.Hit, Nodes.Die),
        //         new Sequence(new TracingNode(), 
        //             new ChargingNode(1f),
        //             new RandomNode(new()
        //             {
        //             //     // 스킬을 사용한 경우 마나와 쿨타임 정보 가지는 방식 필요
        //             //     // 연속 공격이 왜 안됨(애님메이션 관련 문제)
        //             (0.9f, new Sequence( new GunnerRangeAttack(), new IdleNode(0.05f), new GunnerRangeAttack())),
        //             (1f, new GunnerRangeAttack())
        //             }),
        //             new CoolTimeNode(0.5f)), 
        //         new Sequence(new IdleNode(1), new PatrolMove(1)))
        // },
        {
            // 탱커 - 폭팔 발생 시 도주 필요
            Enemy.NightBone.ToString(),
            new Selector(
                new Sequence(new HitNode(), new DieNode()),
                new Sequence(
                    new Sequence( new TracingNode()),
                    // new Condition((controller) => true, new IdleNode(1)),
                    new Sequence(new RandomNode(new()
                    {
                        (0.2f, new Sequence(new ChargingNode(2f), new ExplosionNode())),
                        (1f, new Sequence(new MeleeAttack(), new IdleNode(0.5f)))
                    }))), 
                new Sequence(new IdleNode(1), new PatrolMove(1)))
        },
        // {
        //     // 소드맨 - 속도가 빨라서 원거리 공격이 파훼법(방어가 필요할 듯)
        //     Enemy.SwordShadow.ToString(),
        //     new Selector(
        //         new Sequence(Nodes.Hit, Nodes.Die),
        //         new Sequence(
        //             new TracingNode(),
        //             new SSCastingNode(), new SSDashAttack("Combo1"), new RandomCoolTimeNode(),
        //             new SSCastingNode(), new SSDashAttack("Combo2"), new RandomCoolTimeNode(),
        //             new SSCastingNode(), new SSDashAttack("Combo3"), new RandomCoolTimeNode() 
        //         ),
        //         new Sequence(new IdleNode(1), new PatrolMove(1)))
        // },
        {
            Enemy.Wizard.ToString(),
            new Selector(
                new Sequence(Nodes.Hit, Nodes.Die),
                    new Sequence(  new IdleNode(1.6f), new WizadRecursiveNode(), new IdleNode(1.6f)),
                    new Sequence(new IdleNode(1.6f), new HealNode())
                )
        },
        // {
        //     Enemy.MudEye,
        //     new Selector(
        //         new Sequence(Nodes.Hit, Nodes.Die),
        //         // new SequenceNode(new MudWarpNode(), new MudWarpNode(), new MudWarpNode())
        //         new Sequence(new IdleNode(1), new MudSpawnNode(), new MudWarpNode())
        //         )
        // },
        // {
        //     Enemy.MudHand,
        //     new Selector(
        //         new Sequence(Nodes.Hit, Nodes.Die),
        //         new Sequence(new MudAggroNode(), new MudCastingNode(), new MudAttackNode(), new MudIdleNode())
        //     )
        // },
        {
            Enemy.Bringer.ToString(),
            new Selector(
                new Sequence(Nodes.Hit, Nodes.Die),
                new Sequence(new TracingNode(), new BringerAttackNode(), new IdleNode(1f)),
                new Sequence(new IdleNode(1), new PatrolMove(1))
                )
        },
        // {
        //     Enemy.MoonStone,
        //     new Selector(
        //         new Sequence(Nodes.Hit, Nodes.Die),
        //         // new SequenceNode(new TracingNode(), new StopNode(), new BringerAttackNode(), new IdleNode(1f)),
        //         // new SequenceNode(new MoonCopy1(), new MoonAttack1(), new MoonWarp(), new MoonWarp())
        //         new Sequence(
        //             new MoonAttack1(), new MoonStoneWalk(), new MoonCopyRain3(), new MoonFlyingMode(),  new MoonFlyingAttack(), new MoonFlyingEndNode(), 
        //             new MoonWarp(), new MoonWarp(), new MoonWarp(), new MoonWarp(), new MoonCopy1(), new MoonAttack1(), new MoonCopy2()
        //             )
        //         )
        // }
    };
}