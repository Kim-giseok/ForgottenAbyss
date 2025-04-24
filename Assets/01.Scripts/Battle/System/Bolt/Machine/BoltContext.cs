using System;
using System.Collections.Generic;

public class BoltContext
{
    public Dictionary<string, int> Int { get; private set; } = new();
    public Dictionary<string, float> Float { get; private set; }= new();
    public Dictionary<string, bool> Bool { get; private set; }= new();
    public Dictionary<string, string> String { get; private set; }= new();
    
    public void Set<T>(string name, T value)
    {
        switch (value)
        {
            case int intValue: Int[name] = intValue; break;
            case float floatValue: Float[name] = floatValue; break;
            case bool boolValue: Bool[name] = boolValue; break;
            case string stringValue: String[name] = stringValue; break;
            default: throw new InvalidOperationException($"Unsupported type: {typeof(T)}");
        }
    }
}