using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FieldItem"))
        {
            var fieldItem = collision.GetComponent<FieldItem>();
            if (fieldItem != null && Inventory.Instance.AddItem(fieldItem.GetItem()))
            {
                fieldItem.DestroyItem();
            }
        }
    }
}
