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
    public int maxStack = 99;
    public bool isConsumable;

    [TextArea]
    public string itemDescription; 

    public virtual bool Use()
    {       
        return false;
    }

    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
