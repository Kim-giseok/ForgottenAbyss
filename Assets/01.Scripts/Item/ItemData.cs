using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    public static ItemData itemDataInstance;

    private void Awake()
    {
        itemDataInstance = this;
    }
    public List<Item> itemData = new List<Item>();

    public GameObject fieldItemPrefab;
    public Vector3[] pos; // 아이템 위치 배열

    private void Start()
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject spawnedItem =  Instantiate(fieldItemPrefab, pos[i], Quaternion.identity); // 아이템 생성
            spawnedItem.GetComponent<FieldItem>().SetItem(itemData[Random.Range(0, 1)]);
        }
    }
}
