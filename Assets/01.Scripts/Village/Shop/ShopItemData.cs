using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Shop Item Data")]
public class ShopItemData : ScriptableObject
{
    public Item item;
    public int price;
}
