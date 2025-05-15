using System;using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class FieldItemPool: SingletonLoadRemain<FieldItemPool>
{
    public FieldGoldItem coin;
    private readonly List<FieldGoldItem> _currCoins = new();

    public FieldItem memoryItem;
    private Dictionary<string , Dictionary<string, Item>> ItemList { get; set; } = new();
    private readonly List<FieldItem> _currMemoryItems = new();
    
    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode) {}

    protected override void Init()
    {
        Addressables.LoadAssetsAsync<Item>("Item", null).Completed += (handle) =>
        {
            foreach (var itemSO in handle.Result)
            {
                var currType = itemSO.GetType();
                if (!ItemList.TryGetValue(currType.ToString(), out var dict))
                {
                    dict = new Dictionary<string, Item>();
                    ItemList[currType.ToString()] = dict;
                }
                dict[itemSO.itemName] = itemSO;
            }
        };
    }

    public void CreateItem(string currType, Vector2 position, string itemName)
    {
        var newFieldItem = _currMemoryItems.Find(item => !item.gameObject.activeSelf);
        if (!newFieldItem)
        {
            newFieldItem = Instantiate(memoryItem, transform).GetComponent<FieldItem>();
            _currMemoryItems.Add(newFieldItem);
        }
        
        if (ItemList[currType].TryGetValue(itemName, out var currItemSO))
        {
            newFieldItem.Define(currItemSO);
        }
        else
        {
            newFieldItem.gameObject.SetActive(false);
            Debug.LogWarning("cannot find item SO");
        }
        
        newFieldItem.transform.position = position;

        newFieldItem.gameObject.SetActive(true);
        newFieldItem.Spawn();
    }
    

    public void CreateCoin(Vector2 position, int amount)
    {
        var newCoin = _currCoins.Find(currCoin => !currCoin.gameObject.activeSelf);
        if (newCoin == null)
        {
            newCoin = Instantiate(coin, transform);
            _currCoins.Add(newCoin);
        }

        newCoin.transform.position = position;
        
        newCoin.amount = amount;
        newCoin.gameObject.SetActive(true);
        newCoin.Spawn();
    }
}