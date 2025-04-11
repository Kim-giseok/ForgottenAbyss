using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItem : MonoBehaviour
{
    public Item item; // 필드에 떨어져 있는 아이템

    // 아이템 설정
    public void SetItem(Item newItem)
    {
        item = newItem;
        // 아이템 이미지 설정 등등 추가로 필요한 설정 여기서 처리하기!
    }
    // 아이템 반환
    public Item GetItem()
    {
        return item;
    }

    // 아이템 삭제
    public void DestroyItem()
    {
        Destroy(gameObject);
    }
}
