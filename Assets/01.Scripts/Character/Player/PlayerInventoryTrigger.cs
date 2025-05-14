using UnityEngine;

public class PlayerInventoryTrigger : MonoBehaviour
{
    [SerializeField] private InventoryController inventoryController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FieldItem"))
        {
            SoundManager.Instance.Playsfx("GetItem");
            if (collision.TryGetComponent(out FieldGoldItem goldItem))
            {
                // 오브젝트 풀에서 관리하기
                GoldManager.Instance.AddGold(goldItem.amount);
                collision.gameObject.SetActive(false);
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
