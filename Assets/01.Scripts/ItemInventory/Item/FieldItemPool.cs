using System.Collections.Generic;
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
    public Dictionary<string, MemorySkillItem> MemoryItemList { get; private set; } = new();
    private readonly List<FieldItem> _currMemoryItems = new();
    
    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode) {}

    protected override void Init()
    {
        Addressables.LoadAssetsAsync<MemorySkillItem>("MemoryItem", null).Completed += (handle) =>
        {
            foreach (var memoryItemSO in handle.Result)
            { 
                MemoryItemList.Add(memoryItemSO.name, memoryItemSO);
            }
        };
    }

    public void CreateMemoryItem(Vector2 position, string itemName)
    {
        var newMemoryItem = _currMemoryItems.Find(item => !item.gameObject.activeSelf);
        if (!newMemoryItem)
        {
            newMemoryItem = Instantiate(memoryItem, transform).GetComponent<FieldItem>();
            _currMemoryItems.Add(newMemoryItem);
        }

        if (MemoryItemList.TryGetValue(itemName, out var currSkillItem))
        {
            newMemoryItem.Define(currSkillItem);
        }
        else
        {
            newMemoryItem.gameObject.SetActive(false);
            Debug.LogWarning("cannot find memory skill item SO");
        }
        
        newMemoryItem.transform.position = position;

        newMemoryItem.gameObject.SetActive(true);
        newMemoryItem.Spawn();
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