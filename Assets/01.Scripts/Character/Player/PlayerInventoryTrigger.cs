using UnityEngine;

public class PlayerInventoryTrigger : MonoBehaviour
{
    [SerializeField] private InventoryController inventoryController;

    private void Awake()
    {
        if (inventoryController == null)
        {
            inventoryController = UIManager.Instance?.inventoryUI?.InventoryController;

            if (inventoryController == null)
            {
                Debug.LogError("[PlayerInventoryTrigger] InventoryController를 UIManager에서 가져오지 못했습니다.");
            }
            else
            {
                Debug.Log($"[PlayerInventoryTrigger] InventoryController 자동 연결됨: {inventoryController.gameObject.name}");
            }

        }
    }

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
