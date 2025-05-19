using UnityEngine;

public class ShopTooltipData : ITooltipData
{
    private ShopItemData shopData;

    public ShopTooltipData(ShopItemData data)
    {
        shopData = data;
    }

    public string GetTitle() => shopData.item.itemName;
    public string GetDescription() => $"{shopData.item.itemDescription}\n\n°¡°Ý: {shopData.price:N0} G";
    public Sprite GetIcon() => shopData.item.itemIcon;
}
