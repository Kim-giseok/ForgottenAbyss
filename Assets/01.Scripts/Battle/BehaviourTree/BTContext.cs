using System.Collections.Generic;
public interface IBox { }
public class Box<T> : IBox { public T value; public Box(T value) => this.value = value; }

// 박싱 문제는 사라지나 new Box로 할당하는 문제 발생
public class BTContext 
{
    private Dictionary<string, IBox> data = new();
    public void Set<T>(string key, T value) => data[key] = new Box<T>(value);

    public T Get<T>(string key)
    {
        if (data.TryGetValue(key, out var box) && box is Box<T> typedBox) { return typedBox.value; }
        throw new KeyNotFoundException($"'{key}' is not found in BTContext.");
    }
    public bool Has(string key) => data.ContainsKey(key);
}