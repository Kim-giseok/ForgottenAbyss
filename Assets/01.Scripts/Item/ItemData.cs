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
    public Vector3[] pos;

    private void Start()
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject go =  Instantiate(fieldItemPrefab, pos[i], Quaternion.identity);
            go.GetComponent<FieldItem>().SetItem(itemData[Random.Range(0, 1)]);
        }
    }
}
