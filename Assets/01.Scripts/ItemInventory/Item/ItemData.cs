using UnityEngine;

public class ItemData : MonoBehaviour
{
    public Item[] allItems; // 게임 내 아이템 데이터 목록
    public GameObject fieldItemPrefab; // 필드에 배치할 아이템 프리팹
    public Vector3[] spawnPositions; // 아이템 배치 위치들

    void Start()
    {
        SpawnItems();
    }

    // 필드에 아이템 배치
    void SpawnItems()
    {
        for (int i = 0; i < spawnPositions.Length; i++)
        {
            var item = allItems[Random.Range(0, allItems.Length)]; // 아이템 랜덤 선택
            GameObject spawnedItem = Instantiate(fieldItemPrefab, spawnPositions[i], Quaternion.identity);
            FieldItem fieldItem = spawnedItem.GetComponent<FieldItem>();
            fieldItem.SetItem(item); // 아이템을 필드에 설정
        }
    }
}
