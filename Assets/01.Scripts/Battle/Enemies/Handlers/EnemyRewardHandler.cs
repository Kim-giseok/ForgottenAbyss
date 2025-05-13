using UnityEngine;

// 비용에 따른 아이템 스포너가 필요할 듯
public class EnemyRewardHandler : MonoBehaviour
{
    private EnemyRewardSO _rewardSO;
    public void Define(EnemyRewardSO newRewardSO) => _rewardSO = newRewardSO;
    
    public int Gold => _rewardSO.gold;
    public int Experience => _rewardSO.experience;

    public void DropMemoryItem()
    {
        FieldItemPool.Instance.CreateMemoryItem(transform.position + (Vector3.up * 0.5f), gameObject.name.Replace("(Clone)", ""));
    }

    public void DropCoin()
    {
        if (Gold <= 0) return;
        FieldItemPool.Instance.CreateCoin(transform.position + (Vector3.up * 0.5f), Gold);
    }   
}