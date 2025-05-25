using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static BT;

// 이름으로 BT를 지정하는 부분 고민해보기
public enum Enemy { Test, Agis, Archer, Ghost, GhostChild, Gunner, NightBone, SwordShadow, Wizard, MudEye, MudHand, Bringer, MoonStone, Cathulu }

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
          Selector(
              Sequence(
                  Condition(ctrl => ctrl.statusHandler.GetMode(EnmeyMode.Hit)), 
                  Selector(
                      // 사망한 경우
                      Sequence(
                          Condition(ctrl => ctrl.resourceHandler.Get(EnemyStatType.Health).value <= 0),
                          Do<SetZeroPosNode>(),
                          Do<DieNode>()
                      ).Ignore(),
                      Do<HitNode>() 
                  )
              ),
              Sequence(Do<AgisMoveNode>())
              )
        },
        {
            (int)Enemy.Archer,
            Selector(
                // 피격 당한 경우
                Sequence(
                    Condition(ctrl => ctrl.statusHandler.GetMode(EnmeyMode.Hit)), 
                    Selector(
                        // 사망한 경우
                        Sequence(
                            Condition(ctrl => ctrl.resourceHandler.Get(EnemyStatType.Health).value <= 0),
                            Do<DieNode>().Ignore()
                        ),
                        Do<HitNode>() 
                    )
                ),
                Sequence(
                    Condition(ctrl => ctrl.Board.IsAttacking),
                    Selector(
                        Sequence(
                            Condition(ctrl => ctrl.Agent.status != EnemyAgent.Status.Tracked),
                            Selector(
                                Sequence(
                                    Condition(ctrl => ctrl.detectHandler.isWalkable),
                                    Do<TracingNode>()
                                ),
                                Sequence(
                                    Do<StopNode>(),
                                    Do<ChargingNode>(1.6f),
                                    Do<RangeMultiAttackNode>()
                                ).Ignore()
                            )
                        ),
                        Sequence(
                            Do<StopNode>(),
                            Do<ChargingNode>(1.6f),
                            Do<RangeMultiAttackNode>()
                        ).Ignore()
                    )
                ),
                Sequence(
                    Do<IdleNode>(1f),
                    Do<PatrolMove>(1f)
                )
            )
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
            (int)Enemy.NightBone,
            Selector(
                // 피격 당한 경우
                Sequence(
                    Condition(ctrl => ctrl.statusHandler.GetMode(EnmeyMode.Hit)), 
                    Selector(
                        // 사망한 경우
                        Sequence(
                            Condition(ctrl => ctrl.resourceHandler.Get(EnemyStatType.Health).value <= 0),
                            Do<DieNode>()
                            ),
                        Do<HitNode>() 
                    )
                ),
                // 전투 상태인 경우
                Sequence(
                    Condition(ctrl => ctrl.Board.IsAttacking),
                    Selector(
                        // 추적 중인 경우
                        Sequence(
                            Condition(ctrl => ctrl.Agent.status != EnemyAgent.Status.Tracked),
                            Selector(
                                Sequence(Condition(ctrl => ctrl.detectHandler.isWalkable), Do<TracingNode>()),
                                Do<LookTargetNode>()
                            )
                        ),
                        // 추적 완료한 경우
                        Sequence(
                            Do<StopNode>(),
                            DoRandom(
                                (0.2f, Sequence(Do<ChargingNode>(1f), Do<ExplosionNode>()).Ignore()),
                                (1f, Sequence(Do<MeleeAttack>(), Do<IdleNode>(0.5f)).Ignore())
                            )
                        )
                    )
                ),
                // 일반 상태인 경우
                Sequence(Do<IdleNode>(1f), Do<PatrolMove>(1f))
            )
        },
        {
            (int)Enemy.SwordShadow,
            Selector(
                Sequence(
                    Condition(ctrl => ctrl.statusHandler.GetMode(EnmeyMode.Hit)), 
                    Selector(
                        // 사망한 경우
                        Sequence(
                            Condition(ctrl => ctrl.resourceHandler.Get(EnemyStatType.Health).value <= 0),
                            Do<DieNode>()
                        ),
                        Do<HitNode>() 
                    )
                ),
                Sequence(
                    Condition(ctrl => ctrl.Board.IsAttacking),
                    Selector(
                        Sequence(
                            Condition(ctrl => ctrl.Agent.status != EnemyAgent.Status.Tracked),
                            Selector(
                                Sequence(
                                    Condition(ctrl => ctrl.detectHandler.isWalkable),
                                    Do<TracingNode>()
                                ),
                                Do<LookTargetNode>()
                            )
                        ),
                        DoRandom(
                            (0.3f, Sequence(Do<SSCastingNode>(), Do<SSDashAttack>("Combo1"), Do<RandomCoolTimeNode>()).Ignore()),
                            (0.6f, Sequence(Do<SSCastingNode>(), Do<SSDashAttack>("Combo2"), Do<RandomCoolTimeNode>()).Ignore()),
                            (1f,  Sequence(Do<SSCastingNode>(), Do<SSDashAttack>("Combo3"), Do<RandomCoolTimeNode>()).Ignore())
                        )
                    )
                ),
                Sequence(
                    Do<IdleNode>(1f),
                    Do<PatrolMove>(1f)
                )
            )
        },
        {
            (int)Enemy.Wizard,
            Selector(
                // 피격 당한 경우
                Sequence(
                    Condition(ctrl => ctrl.statusHandler.GetMode(EnmeyMode.Hit)), 
                    Selector(
                        // 사망한 경우
                        Sequence(
                            Condition(ctrl => ctrl.resourceHandler.Get(EnemyStatType.Health).value <= 0),
                            Do<DieNode>()
                        ),
                        Do<HitNode>() 
                    )
                ),
                // 공격 상태인 경우
                Sequence(
                    Condition(ctrl => ctrl.Board.IsAttacking),
                    Do<StopNode>(), Do<IdleNode>(1.6f), Do<WizadRecursiveNode>(), Do<IdleNode>(1.6f))
                ,
                Sequence(Do<IdleNode>(1.6f), Do<HealNode>())
            )
        },
        {
            (int)Enemy.MudEye,
            Selector(
                // 피격 당한 경우
                Sequence(
                    Condition(ctrl => ctrl.statusHandler.GetMode(EnmeyMode.Hit)), 
                    Selector(
                        // 사망한 경우
                        Sequence(
                            Condition(ctrl => ctrl.resourceHandler.Get(EnemyStatType.Health).value <= 0),
                            Do<DieNode>()
                        ),
                        Do<HitNode>() 
                    )
                ),
                Sequence(Do<IdleNode>(1))
            )
        },
        {
            (int)Enemy.MudHand,
            Selector(
                Sequence(
                    Condition(ctrl => ctrl.statusHandler.GetMode(EnmeyMode.Hit)), 
                    Selector(
                        // 사망한 경우
                        Sequence(
                            Condition(ctrl => ctrl.resourceHandler.Get(EnemyStatType.Health).value <= 0),
                            Do<DieNode>()
                        ),
                        Do<HitNode>() 
                    )
                ),
                Sequence(
                    Do<MudSHandpawnNode>(),
                    Do<RandomCoolTimeNode>(),
                    Do<MudHandRangeAttack>()
                )
            )
        },
        {
            (int)Enemy.Bringer,
            Selector(
                // 피격 당한 경우
                Sequence(
                    Condition(ctrl => ctrl.statusHandler.GetMode(EnmeyMode.Hit)), 
                    Selector(
                        // 사망한 경우
                        Sequence(
                            Condition(ctrl => ctrl.resourceHandler.Get(EnemyStatType.Health).value <= 0),
                            Do<DieNode>()
                        ),
                        Do<HitNode>() 
                    )
                ),
                // 전투 중인 경우
                Sequence(
                    Condition(ctrl => ctrl.Board.IsAttacking),
                    Selector(
                        // 추적 중인 경우
                        Sequence(
                            Condition(ctrl => ctrl.Agent.status != EnemyAgent.Status.Tracked),
                            Selector(
                                Sequence(
                                    Condition(ctrl => ctrl.detectHandler.isWalkable),
                                    Do<TracingNode>()
                                ),
                                Do<LookTargetNode>()
                            )
                        ),
                        // 공격 가능한 경우
                        Sequence(
                            Do<StopNode>(),
                            Do<BringerAttackNode>(),
                            Do<IdleNode>(1f)
                        )
                    )
                ),
                // 이동 중인 경우
                Sequence(
                    Do<IdleNode>(1f),
                    Do<PatrolMove>(1f)
                )
            )
        },
        {
            (int)Enemy.Cathulu,
            Selector(
                // 피격 당한 경우
                Sequence(
                    Condition(ctrl => ctrl.statusHandler.GetMode(EnmeyMode.Hit)), 
                    Selector(
                        // 사망한 경우
                        Sequence(
                            Condition(ctrl => ctrl.resourceHandler.Get(EnemyStatType.Health).value <= 0),
                            Do<DieNode>()
                        ),
                        Do<HitNode>() 
                    )
                ),
                // 전투 중인 경우
                Sequence(
                    Condition(ctrl => ctrl.Board.IsAttacking),
                    Selector(
                        // 추적 중인 경우
                        Sequence(
                            Condition(ctrl => ctrl.Agent.status != EnemyAgent.Status.Tracked),
                            Selector(
                                Sequence(
                                    Condition(ctrl => ctrl.detectHandler.isWalkable),
                                    Do<TracingNode>()
                                ),
                                Do<LookTargetNode>()
                            )
                        ),
                        // 공격 가능한 경우
                        Sequence(
                            Do<StopNode>(),
                            Do<MeleeAttack>(),
                            Do<IdleNode>(1f)
                        ).Ignore()
                    )
                ),
                // 이동 중인 경우
                Sequence(
                    Do<IdleNode>(1f),
                    Do<PatrolMove>(1f)
                )
            )
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
