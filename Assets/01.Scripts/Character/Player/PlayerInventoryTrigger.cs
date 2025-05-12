using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FieldItem"))
        {
            SoundManager.Instance.Playsfx("GetItem");
            if (collision.TryGetComponent(out FieldGoldItem goldItem))
            {
                // 오브젝트 풀에서 관리하기
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
