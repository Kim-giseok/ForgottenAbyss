using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance; //싱글톤

    public List<Item> items = new List<Item>(); // 아이템 목록
    public int capacity = 10;

    public event Action onItemChanged; // 슬롯 개수 변경 시 호출


    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // 아이템 추가
    public bool AddItem(Item item)
    {
        if (item == null)
        {
            return false;
        }

        if (items.Count >= capacity)
        {
            return false;
        }

        items.Add(item);
        onItemChanged?.Invoke();
        return true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.LogWarning(collision.gameObject.name);
        if (collision.CompareTag("FieldItem"))
        {
            FieldItem fieldItem = collision.GetComponent<FieldItem>();
            if (AddItem(fieldItem.GetItem()))
            {
                fieldItem.DestroyItem();
            }
        }
    }
}
