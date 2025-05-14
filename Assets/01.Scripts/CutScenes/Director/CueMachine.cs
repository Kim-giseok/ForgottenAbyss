using System;
using System.Collections.Generic;
using UnityEngine;

public class CueMachine
{
    private readonly Queue<Action> _scenario = new();
    public bool IsPlaying { get; private set; }
    private bool IsFinish { get; set; }
    
    public Action OnFinish;
    
    public void Start() => IsPlaying = true;
    
    public void Define(params Action[] actions)
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
        if (_scenario.Count <= 0)
        { 
            IsFinish = true;
            OnFinish?.Invoke();
            return;
        }
        _scenario.Dequeue()?.Invoke();
    }
}