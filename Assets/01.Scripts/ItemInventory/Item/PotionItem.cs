using UnityEngine;

[CreateAssetMenu(fileName = "NewPotionItem", menuName = "SO/Item/PotionItem")]
public class PotionItem : Item
{
    [Header("포션 효과 설정")]
    public StatType targetStat; // 회복할 스탯
    public float healAmount = 30f; // 회복량

    public override bool Use()
    {
        Debug.Log($"[{itemName}] {targetStat} {healAmount}만큼 회복!");

        return true;
    }
}
