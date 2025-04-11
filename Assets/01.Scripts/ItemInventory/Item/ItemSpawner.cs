using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject fieldItemPrefab; // 필드에 생성할 아이템 프리템
    public Item[] itemsToSpawn; // 생성할 아이템들
    public Vector2[] spawnPositions; // 아이템이 생성될 위치

    void Start()
    {
        SpawnItems();
    }

    void SpawnItems()
    {
        for (int i = 0; i < itemsToSpawn.Length && i < spawnPositions.Length; i++)
        {
            GameObject go = Instantiate(fieldItemPrefab, spawnPositions[i], Quaternion.identity);

            FieldItem fieldItem = go.GetComponent<FieldItem>();
            fieldItem.SetItem(itemsToSpawn[i]);

            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            if (sr != null && itemsToSpawn[i].itemIcon != null)
            {
                sr.sprite = itemsToSpawn[i].itemIcon; // 아이콘 설정
            }
        }
    }
}
