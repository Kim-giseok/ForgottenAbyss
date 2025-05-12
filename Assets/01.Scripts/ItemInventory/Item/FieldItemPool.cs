using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class FieldItemPool: SingletonLoadRemain<FieldItemPool>
{
    public FieldGoldItem coin;
    public List<FieldGoldItem> currCoins = new();

    public GameObject memoryItem;
    private Dictionary<string, MemorySkillItem> memoryItemList = new();
    
    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode) {}

    protected override void Init()
    {
        Addressables.LoadAssetsAsync<MemorySkillItem>("MemoryItem", null).Completed += (handle) =>
        {
            foreach (var memoryItemSO in handle.Result)
            { 
                memoryItemList.Add(memoryItemSO.name, memoryItemSO);
            }
        };
    }

    public void CreateCoin(int amount)
    {
        var newCoin = currCoins.Find(currCoin => !currCoin.gameObject.activeSelf);
        if (newCoin == null)
        {
            newCoin = Instantiate(coin);
            currCoins.Add(newCoin);
        }
        
        newCoin.amount = amount;
    }
}