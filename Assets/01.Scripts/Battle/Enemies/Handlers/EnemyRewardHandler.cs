using UnityEngine;

// 비용에 따른 아이템 스포너가 필요할 듯
public class EnemyRewardHandler : MonoBehaviour
{
    private EnemyRewardSO _rewardSO;
    public void Define(EnemyRewardSO newRewardSO) => _rewardSO = newRewardSO;
    
    public int Gold => _rewardSO.Gold;
    public int Experience => _rewardSO.Experience;

    public void DropMemoryItem()
    {
        FieldItemPool.Instance.CreateItem(typeof(MemorySkillItem).ToString(), transform.position + (Vector3.up * 0.5f), _rewardSO.memorySkillItem.itemName);
    }

    public void DropItem()
    {
        int currPercent = Random.Range(0, 100);
        
        foreach (var itemInfo in _rewardSO.rewardItemInfos)
        {
            if (currPercent <= itemInfo.percent)
            {
                var currType = itemInfo.item.GetType();
                FieldItemPool.Instance.CreateItem(currType.ToString(), transform.position + (Vector3.up * 0.5f), itemInfo.item.itemName);
            }
        }
    }

    public void DropCoin()
    {
        if (Gold <= 0) return;
        FieldItemPool.Instance.CreateCoin(transform.position + (Vector3.up * 0.5f), Gold);
    }   
}