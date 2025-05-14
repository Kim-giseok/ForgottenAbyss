using UnityEngine;

public class PlayerInventoryTrigger : MonoBehaviour
{
    [SerializeField] private InventoryController inventoryController;

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
            if (fieldItem != null)
            {
                var item = fieldItem.GetItem();
                bool added = inventoryController.AddItem(item, 1);
                if (added)
                    fieldItem.DestroyItem();
            }
        }
    }
}
