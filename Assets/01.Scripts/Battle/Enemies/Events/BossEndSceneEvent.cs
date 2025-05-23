using Unity.VisualScripting;
using UnityEngine;

public class BossEndSceneEvent: MonoBehaviour
{
    public GameObject endScene;
    public GameObject enemyPool;
    
    private void OnDisable()
    {
        var fieldItem = FieldItemPool.Instance.CurrMemoryItems.Find(item => item.item.itemName == "기억의 조각");
        var memoryItemEvent = fieldItem.AddComponent<BossMemoryItemEvent>();
        memoryItemEvent.SetEvent(() => endScene.SetActive(true));
        
        if(enemyPool) { enemyPool.gameObject.SetActive(false); }
        BoltsPool.Instance.Clear();
    }
}