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

        if (player == null)
        {
            Debug.LogWarning("플레이어를 찾을 수 없습니다.");
            return false;
        }

        float current = player.GetStat(targetStat);
        float max = player.GetStat(GetMaxStatType(targetStat));

        if (current >= max)
        {
            Debug.Log($"[{itemName}] {targetStat}이 이미 최대입니다.");
            return false;
        }

        float newValue = Mathf.Min(current + healAmount, max);
        player.SetStat(targetStat, newValue);

        Debug.Log($"[{itemName}] {targetStat}을(를) {healAmount}만큼 회복! → {newValue}/{max}");

        return true;
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
