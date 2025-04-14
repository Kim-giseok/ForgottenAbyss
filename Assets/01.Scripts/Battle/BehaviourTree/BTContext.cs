using System.Collections.Generic;

// 박싱 문제 발생
public class BTContext 
{
    private Dictionary<string, object> data = new();

    public void Set<T>(string key, T value) => data[key] = value;

    public T Get<T>(string key) {
        if (data.TryGetValue(key, out var value)) {
            return (T)value;
        }

        throw new KeyNotFoundException($"'{key}' is not found in BTContext.");
    }

    public bool TryGet<T>(string key, out T value) {
        if (data.TryGetValue(key, out var obj) && obj is T casted) {
            value = casted;
            return true;
        }

        value = default;
        return false;
    }

    public bool Has(string key) => data.ContainsKey(key);
}