using System;
using System.Collections.Generic;
using UnityEngine;

public class NightBoneAttackProvider
{
    public static Action<Node> GetAttackNode(string index)
    {
        return index switch
        {
            // 익명함수 비용 발생
            "explosion" => (node) =>
            {
                BoltsPool.Instance.CreateMelee(node.controller.transform)
                    .SetLocalPos(Vector2.zero)
                    .SetDamage(20)
                    .SetSize(2f)
                    .Fire();
            },
            _ => throw new ArgumentOutOfRangeException(nameof(index), index, null)
        };
    }
}

public class ArcherAttackProvider
{
    public static Action<Node> GetAttackNode(string actionName)
    {
        return actionName switch
        {
            "base" => (node) =>
            {
                for (int currDegree = -20; currDegree <= 20; currDegree += 10)
                {
                    if (node.controller is EnemyController)
                    {
                        BoltsPool.Instance.Create(node.controller.transform, Bolts.Type.Decrescendo)
                            .SetSprite("arrow")
                            .SetSize(1f)
                            .SetDamage(10)
                            .SetSpeed(30)
                            .SetDegree(node.controller.agent.GetDegree() + currDegree)
                            .SetDuration(0.6f)
                            .Fire();
                    }
                }
            },
            // 같은 스킬 사용자에 따라 달라지는 경우
            "blast" => (node) =>
            {
                for (int currDegree = 0; currDegree <= 360; currDegree += 30)
                {
                    BoltsPool.Instance.Create(node.controller.transform, Bolts.Type.Decrescendo)
                        .SetSprite("arrow")
                        .SetSize(1f)
                        .SetDamage(4)
                        .SetKnockBack(4)
                        .SetEffect(Bolts.EffectType.Penetration)
                        .SetSpeed(60)
                        .SetDegree(currDegree)
                        .SetDuration(0.4f)
                        .Fire();
                }
            }
        };
    }
}

public class AgisAttackProvider
{
    public static Action<Node> GetAttackNode(string actionName)
    {
        return actionName switch
        {
            "spread" => (node) =>
            {
                Vector2[] directions = { Vector2.up, Vector2.down, Vector2.right, Vector2.left };

                foreach (var dir in directions)
                {
                    BoltsPool.Instance
                        .Create(node.controller.transform, Bolts.Type.Linear)
                        .SetDirection(dir)
                        .SetSpeed(16f)
                        .SetDamage(10)
                        .Fire();
                }
            },
            // 일정 주기를 가지는 것에 대해 어케해야할까? - SO에서 관리?
            "rain" => (node) =>
            {
                if (!Mathf.Approximately(Mathf.Floor(node.currTime / 0.2f), Mathf.Floor((node.currTime - Time.deltaTime) / 0.2f)))
                {
                    BoltsPool.Instance.Create(node.controller.transform, Bolts.Type.Rain).SetEffect(Bolts.EffectType.Penetration).SetDamage(8).Fire();
                }
            },
            "balckHole" => (node) =>
            {
                BoltsPool.Instance.Create(node.controller.transform, Bolts.Type.BlackHole).SetEffect(Bolts.EffectType.Penetration)
                    .SetDirection(Vector2.down * 6f).SetDuration(4).Fire();
            }
        };
    }
}

public class BringerAttackProvider
{
    public static Action<Node> GetAttackNode(string actionName)
    {
        return actionName switch
        {
            "melee" => (node) =>
            {
                BoltsPool.Instance.CreateMelee(node.controller.transform).SetDamage(10).SetKnockBack(20).SetSize(3f, 2f).Fire();
            }
        };
    }
}

public class MudAttackProvider
{
    public static Action<Node> GetAttackNode(string actionName)
    {
        return actionName switch
        {
            "melee" => (node) =>
            {
                foreach (Platform platform in NavSurface.Instance.platforms)
                {
                    BoltsPool.Instance.CreateSummon(node.controller.transform, SummonSkillManager.Skill.MudWave, false).SetPosition(platform.startCell.WorldPos + new Vector2(0, 1.5f)).Fire();
                }            
            },
            "spawn" => (node) =>
            {
                var currPos = NavSurface.Instance.GetPlatform(node.controller.agent.target).centerCell.WorldPos + new Vector2(0, 1.7f);
                EnemyRespawner.Instance.Create(EnemiesBT.Enemy.MudHand, currPos);
            }
        };
    }
}

public class WizardAttackProvider
{
    public static Action<Node> GetAttackNode(string actionName)
    {
        return actionName switch
        {
            "melee" => (node) =>
            {
                BoltsPool.Instance.Create(node.controller.transform, Bolts.Type.Recursive)
                    .SetSize(0.6f).SetDamage(10).SetSpeed(4).SetDegree(node.controller.agent.GetDegree())
                    .SetEffect(Bolts.EffectType.Penetration).SetDuration(1.6f).Fire();
            },
            "heal" => (node) =>
            {
                Collider2D[] hits = Physics2D.OverlapCircleAll(node.controller.transform.position, 200f, LayerMask.GetMask("Enemy"));
                foreach (var hit in hits)
                {
                    if (hit.gameObject == node.controller.gameObject) continue;
                    if (!hit.TryGetComponent(out EnemyController econtoller)) continue;
                    BoltsPool.Instance.Create(hit.transform, Bolts.Type.Heal).SetSize(1f).Fire();
                    if(econtoller.health <= 30) econtoller.health += 10;
                }
            }
        };
    }
}