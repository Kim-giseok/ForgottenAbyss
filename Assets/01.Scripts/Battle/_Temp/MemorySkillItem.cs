using UnityEngine;

[CreateAssetMenu(menuName = "Item/MemorySkillItem")]
public class MemorySkillItem: Item
{
    public SummonSkillManager.Skill skillName;
    public int memoryPieceId;
    public float coolTime;
    public bool isRide;

    public override bool Use()
    {
        BoltsPool.Instance.CreateSummon(GameManager.Instance.player.transform, skillName, isRide);
        return true;
    }
}