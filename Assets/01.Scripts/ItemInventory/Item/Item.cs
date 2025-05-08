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
    public ItemType itemType;

    [TextArea]
    public string itemDescription;

    [HideInInspector] public int currentAmount = 1; // 현재 수량
    public int maxStack = 99; // 최대 스텍 수

    public virtual bool Use()
    {       
        return false;
    }

    public override bool Equals(object obj)
    {
        if (obj is Item other)
        {
            return this.itemName == other.itemName;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return itemName.GetHashCode();
    }
}
