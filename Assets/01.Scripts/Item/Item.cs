using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName;
    public Sprite itemIcon;
    public int skillId;

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
