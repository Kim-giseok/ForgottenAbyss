using Unity.VisualScripting;
using UnityEngine;

public class BossEndSceneEvent: MonoBehaviour
{
    public GameObject endScene;
    public GameObject enemyPool;
    
    private void OnDisable()
    {
        // 씬이 언로드되고 있는 중이면 실행 안 함
        if (!gameObject.scene.isLoaded) return;
        
        var fieldItem = FieldItemPool.Instance.CurrMemoryItems.Find(item => item.item.itemName == "기억의 조각");
        var memoryItemEvent = fieldItem.AddComponent<BossMemoryItemEvent>();
        memoryItemEvent.SetEvent(() => endScene.SetActive(true));
        
        if(enemyPool) { enemyPool.gameObject.SetActive(false); }
        BoltsPool.Instance.Clear();
    }
}