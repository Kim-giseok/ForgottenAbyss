using UnityEngine;

public enum ItemType
{
    Equip,
    Consumable,
    Etc,
    Memory
}

[CreateAssetMenu (menuName = "Item/ItemData")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public string itemDescription;
    public ItemType itemType;

    public virtual bool Use()
    {       
        return false;
    }
}
