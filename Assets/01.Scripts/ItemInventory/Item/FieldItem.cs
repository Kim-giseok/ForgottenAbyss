using System;
using System.Collections.Generic;
using UnityEngine;

public class FieldItem : MonoBehaviour
{
    public Item item; // �ʵ忡 ������ �ִ� ������
    
    private readonly List<SpriteRenderer> _renderers = new();
    private FieldItemDropAnimator _dropAnimator;

    private void Awake()
    {
        _dropAnimator = GetComponent<FieldItemDropAnimator>();
    }

    public void Define(Item newItem)
    {
        item = newItem;
        _renderers.Add(transform.GetChild(0).GetComponent<SpriteRenderer>());
        _renderers.Add(transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>());
        _renderers.ForEach(spriteRenderer => spriteRenderer.sprite = item.itemIcon);
    }

    // ������ ����
    public void SetItem(Item newItem)
    {
        item = newItem;
    }
    // ������ ��ȯ
    public Item GetItem()
    {
        return item;
    }

    // ������ ����
    public void DestroyItem()
    {
        gameObject.SetActive(false);
    }

    public void Spawn()
    {
        _dropAnimator.Spawn();
    }
}
