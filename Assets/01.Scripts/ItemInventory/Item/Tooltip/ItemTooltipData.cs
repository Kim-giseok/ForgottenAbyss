using UnityEngine;

public class ItemTooltipData : ITooltipData
{
    private Item item;

    public ItemTooltipData(Item item)
    {
        this.item = item;
    }

    public string GetTitle() => item.itemName;
    public string GetDescription() => item.itemDescription;
    public Sprite GetIcon() => item.itemIcon;
}
