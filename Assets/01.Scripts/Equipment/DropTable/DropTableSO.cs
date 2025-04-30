using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DropTable/New Drop Table")]
public class DropTableSO : ScriptableObject
{
    public List<DropEntry> dropEntries;

    public List<Item> GetDroppedItems()
    {
        List<Item> results = new List<Item>();
        foreach (var entry in dropEntries)
        {
            if (UnityEngine.Random.value <= entry.dropChance)
            {
                results.Add(entry.item);
            }
        }
        return results;
    }
}
