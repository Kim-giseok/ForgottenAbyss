using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class BossEndSceneEvent: MonoBehaviour
{
    public GameObject endScene;
    private void OnDisable()
    {
        var fieldItem = FieldItemPool.Instance.CurrMemoryItems.Find(item => item.item.itemName == "기억의 조각");
        var memoryItemEvent = fieldItem.AddComponent<AgisMemoryItemEvent>();
        memoryItemEvent.SetEvent(() => endScene.SetActive(true));
    }
}