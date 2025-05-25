using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// 이름으로 BT를 지정하는 부분 고민해보기
public enum Enemy { Test, Agis, Archer, Ghost, GhostChild, Gunner, NightBone, SwordShadow, Wizard, MudEye, MudHand, Bringer, MoonStone }

public class EnemiesBT
{
    // notice: enum을 추가하면 한칸씩 밀리는 현상 발생
    public static Node Get(Enemy enemy) => behaviour[(int)enemy];

    private static Dictionary<int, Node> behaviour = new()
    {
        {
            (int)Enemy.Test,
            new SelectorNode(new IdleNode(1f))
        },
        // notice: 타격이 발생하면 순차 순회를 통해서 다른 스킬 사용되지 않는 현상 발생
        {
          (int)Enemy.Agis,
          new SelectorNode(
              new SequenceNode(new HitNode(), new SetZeroPosNode(), new DieNode()),
              // new SequenceNode(new BlackHoleNode(), new AgisTriangleNode(), new AgisMoveNode(3), new AgisMoveNode(-3))
              new SequenceNode(new AgisMoveNode())
              )
        },
        {
            (int)Enemy.Archer,
            new BTBuilder()
                .Selector()
                    .Sequence().Do<HitNode>().Do<DieNode>().End()
                    .Condition(ctrl => ctrl.Board.IsAttacking)
                        .Selector()
                            .Condition(ctrl => ctrl.Agent.status != EnemyAgent.Status.Tracked)
                                .Selector()
                                    .Condition(ctrl => ctrl.detectHandler.isWalkable).Do<TracingNode>().End()
                                    .Sequence(true).Do<StopNode>().Do<ChargingNode>(1.6f).Do<RangeMultiAttackNode>().End()
                                .End()
                            .End()
                            .Sequence(true).Do<StopNode>().Do<ChargingNode>(1.6f).Do<RangeMultiAttackNode>().End()
                        .End()
                    .End()
                    .Sequence().Do<IdleNode>(1).Do<PatrolMove>(1).End()
                .End()
                .Build()
        },
        {
            (int)Enemy.Ghost,
            new SelectorNode(
                new SequenceNode(new HitNode(), new DieNode()),
                new SequenceNode(new IdleNode(1), new PatrolMove(1))
            )
        },
        {
            // 관통 + 튕기는 효과를 주로 다루는
            (int)Enemy.Gunner,
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
                    })), 
                new SequenceNode(new IdleNode(1), new PatrolMove(1)))
        },
        {
            // 탱커 - 폭팔 발생 시 도주 필요
            (int)Enemy.NightBone,
            new BTBuilder()
               .Selector()
                    .Sequence().Do<HitNode>().Do<DieNode>().End()
                    .Condition(ctrl => ctrl.Board.IsAttacking)
                        .Selector()
                            .Condition(ctrl => ctrl.Agent.status != EnemyAgent.Status.Tracked)
                                .Selector()
                                    .Condition(ctrl => ctrl.detectHandler.isWalkable).Do<TracingNode>().End()
                                    .Do<LookTargetNode>()
                                .End()
                            .End()
                            .Sequence()
                                .Do<StopNode>()
                                .Do(() => new RandomNode(new List<(float, Node)>
                                {
                                    (0.2f, new SequenceNode(new ChargingNode(1f), new ExplosionNode()).Ignore()),
                                    (1f, new SequenceNode(new MeleeAttack(), new IdleNode(0.5f)).Ignore())
                                }))
                            .End()
                        .End()
                   .End()
                   .Sequence().Do<IdleNode>(1f).Do<PatrolMove>(1f).End()
               .End()
               .Build()
        },
        {
            // 소드맨 - 속도가 빨라서 원거리 공격이 파훼법(방어가 필요할 듯)
            (int)Enemy.SwordShadow,
            new BTBuilder()
                .Selector()
                    .Sequence().Do<HitNode>().Do<DieNode>().End()
                    .Condition(ctrl => ctrl.Board.IsAttacking)
                    .Selector()
                        .Condition(ctrl => ctrl.Agent.status != EnemyAgent.Status.Tracked)
                            .Selector()
                                .Condition(ctrl => ctrl.detectHandler.isWalkable).Do<TracingNode>().End()
                                .Do<LookTargetNode>()
                            .End()
                        .End()
                        .Do(() => new RandomNode(new List<(float, Node)>
                        {
                            (0.3f, new SequenceNode(new SSCastingNode(), new SSDashAttack("Combo1"), new RandomCoolTimeNode()).Ignore()),
                            (0.6f, new SequenceNode(new SSCastingNode(), new SSDashAttack("Combo2"), new RandomCoolTimeNode()).Ignore()),
                            (1f,  new SequenceNode(new SSCastingNode(), new SSDashAttack("Combo3"), new RandomCoolTimeNode()).Ignore())
                        }))
                        .End()
                    .End()
                    .Sequence().Do<IdleNode>(1).Do<PatrolMove>(1).End()
                .End()
                .Build()
        },
        {
            (int)Enemy.Wizard,
            new SelectorNode(
                new SequenceNode(new HitNode(), new DieNode()), 
                    new SequenceNode( new StopNode(), new IdleNode(1.6f), new WizadRecursiveNode(), new IdleNode(1.6f)),
                    new SequenceNode(new IdleNode(1.6f), new HealNode())
                )
        },
        {
            (int)Enemy.MudEye,
            new SelectorNode(
                new SequenceNode(new HitNode(), new DieNode()),
                // new SequenceNode(new MudWarpNode(), new MudWarpNode(), new MudWarpNode())
                // new SequenceNode(new IdleNode(1), new MudWarpNode())
                new SequenceNode(new IdleNode(1))
                )
        },
        {
            (int)Enemy.MudHand,
            new SelectorNode(
                new SequenceNode(new HitNode(), new DieNode()),
                new SequenceNode(new MudSHandpawnNode(), new RandomCoolTimeNode(), new MudHandRangeAttack())
                // new SequenceNode(new MudSHandpawnNode(), new MudCastingNode(), new MudAttackNode(), new MudIdleNode())
            )
        },
        // {
        //     (int)Enemy.Bringer,
        //     new SelectorNode(
        //         new SequenceNode(
        //             new HitNode(), 
        //             new DieNode()
        //         ),
        //         new ConditionNode(
        //             controller => controller.Board.IsAttacking, 
        //             new SelectorNode(
        //                 // 추격 중인 경우,
        //                 new ConditionNode(
        //                     ctrl => ctrl.Agent.status != EnemyAgent.Status.Tracked,
        //                     new SelectorNode(
        //                         new ConditionNode(ctrl => ctrl.detectHandler.isWalkable, new TracingNode()),
        //                         // 범위에서 벗어날 경우 그냥 공격
        //                         new LookTargetNode()
        //                     )
        //                 ),
        //                 // 공격 중인 경우
        //                 new SequenceNode(new StopNode(), new BringerAttackNode(), new IdleNode(1f))
        //             )),
        //         new SequenceNode(
        //             new IdleNode(1), 
        //             new PatrolMove(1))
        //     )
        // },
        {
            (int)Enemy.Bringer,
            new BTBuilder()
                    .Selector()
                        // 피격 당한 경우
                        .Sequence().Do<HitNode>().Do<DieNode>().End()
                        // 전투 중인 경우
                        .Condition(ctrl => ctrl.Board.IsAttacking)
                            .Selector()
                                // 추적 중인 경우
                                .Condition(ctrl => ctrl.Agent.status != EnemyAgent.Status.Tracked)
                                    .Selector()
                                        .Condition(ctrl => ctrl.detectHandler.isWalkable).Do<TracingNode>().End()
                                        .Do<LookTargetNode>()   
                                    .End()
                                .End()
                                // 공격 가능한 경우
                                .Sequence().Do<StopNode>().Do<BringerAttackNode>().Do<IdleNode>(1).End()
                            .End()
                        .End()
                        // 이동 중인 경우
                        .Sequence().Do<IdleNode>(1).Do<PatrolMove>(1).End()
                    .End()
                .Build()
        }
        // {
        //     (int)Enemy.MoonStone,
        //     new SelectorNode(
        //         new SequenceNode(new HitNode(), new DieNode()),
        //         // new SequenceNode(new TracingNode(), new StopNode(), new BringerAttackNode(), new IdleNode(1f)),
        //         // new SequenceNode(new MoonCopy1(), new MoonAttack1(), new MoonWarp(), new MoonWarp())
        //         new SequenceNode(
        //             new MoonAttack1(), new MoonStoneWalk(), new MoonCopyRain3(), new MoonFlyingMode(),  new MoonFlyingAttack(), new MoonFlyingEndNode(), 
        //             new MoonWarp(), new MoonWarp(), new MoonWarp(), new MoonWarp(), new MoonCopy1(), new MoonAttack1(), new MoonCopy2()
        //             )
        //         )
        // }
    };
}
