using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CueMachine
{
    private readonly Queue<Func<UniTask>> _scenario = new();
    public bool IsPlaying { get; private set; }
    private bool IsFinish { get; set; }
    
    public Action OnFinish;
    
    public void Start() => IsPlaying = true;
    
    public void Define(params Func<UniTask>[] actions)
    {
        IsFinish = false;
        _scenario.Clear();
        
        foreach (var action in actions)
        {
            _scenario.Enqueue(action);
        }
    }

    public void Next()
    {
        if (IsFinish) return;
        _scenario.Dequeue()?.Invoke();
        if (_scenario.Count == 0)
        { 
            IsFinish = true;
            OnFinish?.Invoke();
        }
    }
}