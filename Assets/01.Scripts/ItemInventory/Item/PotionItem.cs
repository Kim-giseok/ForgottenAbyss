using UnityEngine;

[CreateAssetMenu(fileName = "NewPotionItem", menuName = "SO/Item/PotionItem")]
public class PotionItem : ConsumableItem
{
    [Header("포션 효과 설정")]
    public StatType targetStat; // 회복할 스탯
    public float healAmount = 30f; // 회복량

    public override bool Use()
    {
        PlayerStatus player = GameObject.FindObjectOfType<PlayerStatus>();
        if (player == null) return false;

        float current = player.GetStat(targetStat);
        float max = player.GetStat(GetMaxStatType(targetStat));

        if (current >= max) return false;

        float newValue = Mathf.Min(current + healAmount, max);
        player.SetStat(targetStat, newValue);

        currentAmount--;

        Debug.Log($"[{itemName}] {targetStat} +{healAmount} (남은 수량: {currentAmount})");

        return currentAmount <= 0; // 0 되면 제거
    }

    private StatType GetMaxStatType(StatType stat)
    {
        return stat switch
        {
            StatType.CurrentHP => StatType.MaxHP,
            StatType.CurrentMP => StatType.MaxMP,
            _ => stat
        };
    }
}
