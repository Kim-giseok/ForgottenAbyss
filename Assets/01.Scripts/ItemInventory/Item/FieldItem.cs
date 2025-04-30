using UnityEngine;

public class FieldItem : MonoBehaviour
{
    public Item item; // �ʵ忡 ������ �ִ� ������
    public SpriteRenderer itemImg;

    // ������ ����
    public void SetItem(Item newItem)
    {
        item = newItem;
        itemImg.sprite = item.itemIcon;
    }
    // ������ ��ȯ
    public Item GetItem()
    {
        return item;
    }

    // ������ ����
    public void DestroyItem()
    {
        Destroy(gameObject);
    }
}
