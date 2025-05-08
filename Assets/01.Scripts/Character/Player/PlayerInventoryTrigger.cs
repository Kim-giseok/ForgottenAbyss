using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FieldItem"))
        {
            if (collision.TryGetComponent(out FieldGoldItem goldItem))
            {
                GoldManager.Instance.AddGold(goldItem.amount);
                Destroy(goldItem.gameObject);
                return;
            }
            
            var fieldItem = collision.GetComponent<FieldItem>();
            if (fieldItem != null && Inventory.Instance.AddItem(fieldItem.GetItem()))
            {
                fieldItem.DestroyItem();
            }
        }
    }
}
