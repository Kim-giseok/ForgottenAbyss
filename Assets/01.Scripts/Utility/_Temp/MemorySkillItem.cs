using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "Item/MemorySkillItem")]
public class MemorySkillItem: Item
{
    public SummonSkillManager.Skill skillName;
    public int memoryPieceId;
    public float coolTime;
    public int MpCost;
    public bool isRide;
    public bool isRepeat;
    public int repeatCount = 1;

    public override bool Use()
    {
        if (isRepeat)
        {
            UseRepeat();
        }
        else
        {
            BoltsPool.Instance.CreateSummon(GameManager.Instance.player.transform, skillName, isRide).Fire();

        }

        return true;
    }

    private void UseRepeat()
    {
        switch (skillName)
        {
            case SummonSkillManager.Skill.Agis:
                UseAgisPattern();
                break;
            case SummonSkillManager.Skill.MudWave:
                UseMudWavePattern();
                break;
            default:
                Debug.LogWarning($"[MemorySkillItem] Unsupported repeat skill: {skillName}");
                break;
        }
    }

    private void UseAgisPattern()
    {
        SoundManager.Instance.Playsfx("AgisSpell");
        for (int i = 0; i < repeatCount; i++)
        {
            float angle = i * 40 * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 32;

            BoltsPool.Instance
                .CreateSummon(GameManager.Instance.player.transform, SummonSkillManager.Skill.Agis)
                .SetTrigger(true)
                .SetCastingDirection(direction)
                .Fire();
        }
    }

    private async UniTaskVoid SummonWithIntervalAsync(float delay, int count)
    {
        for (var i = 0; i < count; i++)
        {
            BoltsPool.Instance
                .CreateSummon(GameManager.Instance.player.transform, SummonSkillManager.Skill.MudEye)
                .SetPosition(GameManager.Instance.player.transform.position + new Vector3(Random.Range(-6f, 6f), Random.Range(-2f, 2f), 1f))
                .SetTrigger(true)
                .Fire();

            await UniTask.Delay(TimeSpan.FromSeconds(delay));
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void UseMudWavePattern()
    {
        SummonWithIntervalAsync(0.05f, 13).Forget();

        
        // if (NavSurface.Instance == null)
        // {
        //     Debug.LogError("[MemorySkillItem] NavSurface.Instance is null!");
        //     return;
        // }
        //
        // if (NavSurface.Instance.platforms == null || NavSurface.Instance.platforms.Count == 0)
        // {
        //     Debug.LogError("[MemorySkillItem] NavSurface platforms are empty or null.");
        //     return;
        // }
        //
        // foreach (Platform platform in NavSurface.Instance.platforms)
        // {
        //     if (platform == null || platform.startCell == null)
        //     {
        //         Debug.LogWarning("[MemorySkillItem] Platform or startCell is null, skipping.");
        //         continue;
        //     }
        //
        //     BoltsPool.Instance
        //         .CreateSummon(GameManager.Instance.player.transform, SummonSkillManager.Skill.MudWave, false)
        //         .SetPosition(platform.startCell.WorldPos + new Vector2(0, 1.5f))
        //         .Fire();
        // }
        //
        // BoltsPool.Instance
        //     .CreateSummon(GameManager.Instance.player.transform, SummonSkillManager.Skill.MudEye, true)
        //     .Fire();
    }
}