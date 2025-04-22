using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Item/ItemData")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public string itemDescription;
    // public ItemType itemType;

    public virtual bool Use()
    {
        return false; //������ ��� ���� ���� ��ȯ
    }

}
