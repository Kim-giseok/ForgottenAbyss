using UnityEngine;

[CreateAssetMenu(menuName = "Item/Consumable")]
public class ConsumableItem : Item
{

    private void OnEnable()
    {
        itemType = ItemType.Consumable;
    }

    public override bool Use()
    {
        return true;
    }
}
