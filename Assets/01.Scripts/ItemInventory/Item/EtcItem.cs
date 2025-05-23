using UnityEngine;

[CreateAssetMenu(fileName = "NewEtcItem", menuName = "SO/Item/EtcItem")]
public class EtcItem : Item
{
    private void OnEnable()
    {
        itemType = ItemType.Etc;
        isConsumable = false;
    }
    public override bool Use()
    {
        Debug.Log($"[EtcItem] {itemName}은(는) 사용 불가능한 기타 아이템입니다.");
        return false;
    }
}
