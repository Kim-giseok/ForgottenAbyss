using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTemporaryStatBoostItem", menuName = "SO/Item/TemporaryStatBoostItem")]
public class TemporaryStatBoostItem : ConsumableItem
{
    [Header("능력치 설정")]
    public StatType targetStat;
    public float boostAmount = 10f;
    public float duration = 10f;

    private static HashSet<StatType> activeBuffs = new HashSet<StatType>();

    public override bool Use()
    {
        PlayerStatus player = GameObject.FindObjectOfType<PlayerStatus>();
        if (player == null) return false;

        if (activeBuffs.Contains(targetStat)) 
        {
            Debug.LogWarning($"[StatBoost] {targetStat} 버프는 이미 적용 중입니다!");
            return false; 
        }

        player.StartCoroutine(ApplyTemporaryBoost(player));
        return true;
    }

    private System.Collections.IEnumerator ApplyTemporaryBoost(PlayerStatus player)
    {
        activeBuffs.Add(targetStat);

        float originalValue = player.GetStat(targetStat);
        player.SetStat(targetStat, originalValue + boostAmount);
        Debug.Log($"[TemporaryStatBoostItem] {targetStat} 스탯이 {boostAmount}만큼 증가했습니다. {duration}초 동안 유지됩니다.");

        yield return new WaitForSeconds(duration);

        float currentValue = player.GetStat(targetStat);
        player.SetStat(targetStat, currentValue - boostAmount);
        activeBuffs.Remove(targetStat);

        Debug.Log($"[TemporaryStatBoostItem] {targetStat} 스탯 증가 효과가 종료되었습니다.");

    }
}
