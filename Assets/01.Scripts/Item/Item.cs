using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Item/ItemData")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    // public ItemType itemType;
    public int skillId;

    public bool Use()
    {
        return false; //아이템 사용 성공 여부 반환
    }

    public Item(string name, Sprite icon, int skillId)
    {
        this.itemName = name;
        this.itemIcon = icon;
        this.skillId = skillId;
    }
}
