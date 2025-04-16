using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance; // �̱���

    public List<Item> items = new List<Item>(); // ������ ���
    public int capacity = 10;

    public event Action onItemChanged; // ���� ���� ���� �� ȣ��


    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // ������ �߰�
    public bool AddItem(Item item)
    {
        if (item == null)
        {
            Debug.LogWarning("AddItem() ����: item�� null�Դϴ�!");
            return false;
        }

        if (items.Count >= capacity)
        {
            Debug.LogWarning("AddItem() ����: �κ��丮 ���� ��!");
            return false;
        }

        Debug.Log("������ �߰���: " + item.name);
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
