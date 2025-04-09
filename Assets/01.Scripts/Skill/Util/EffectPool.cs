using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPool : Singleton<EffectPool>
{
    [System.Serializable]
    public class EffectEntry
    {
        public string key;
        public GameObject prefab;
        public int initialSize = 5;
    }

    public List<EffectEntry> effectList;
    private Dictionary<string, GameObjectPool> effectPools = new();

    private void Awake()
    {
        foreach (var entry in effectList)
        {
            effectPools[entry.key] = new GameObjectPool(entry.prefab, entry.initialSize, transform);
        }
    }

    public GameObject SpawnEffect(string key, Vector3 pos, Quaternion rot)
    {
        if (!effectPools.TryGetValue(key, out var pool))
        {
            Debug.LogWarning($"EffectPool not found for key: {key}");
            return null;
        }

        var obj = pool.Get();
        obj.transform.position = pos;
        obj.transform.rotation = rot;
        return obj;
    }

    public void ReleaseEffect(string key, GameObject effect)
    {
        if (effectPools.TryGetValue(key, out var pool))
        {
            pool.Release(effect);
        }
        else
        {
            Destroy(effect);
        }
    }
}
