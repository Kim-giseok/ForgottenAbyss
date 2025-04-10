using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equipment,
    Consumable,
    SkillSlot
}

[System.Serializable]
public class Item
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemIcon;
    public int skillId;

    public bool Use()
    {
        return false; //아이템 사용 성공 여부 반환
    }

    public Item(string name)
    {
        itemName = name;
        itemIcon = null; // 임시로 null로 설정
    }
    public Item(string name, Sprite icon, int skillId)
    {
        this.itemName = name;
        this.itemIcon = icon;
        this.skillId = skillId;
    }
}
