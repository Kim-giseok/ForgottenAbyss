using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Item/ItemData")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    // public ItemType itemType;

    public bool Use()
    {
        return false; //아이템 사용 성공 여부 반환
    }

}
