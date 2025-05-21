using UnityEngine;

[CreateAssetMenu(fileName = "NewTemporaryStatBoostItem", menuName = "SO/Item/TemporaryStatBoostItem")]
public class TemporaryStatBoostItem : ConsumableItem
{
    [Header("능력치 설정")]
    public StatType targetStat;
    public float boostAmount = 10f;
    public float duration = 10f;

    public override bool Use()
    {
        PlayerStatus player = GameObject.FindObjectOfType<PlayerStatus>();
        return player != null && player.TryApplyBuff(targetStat, boostAmount, duration);
    }
}
