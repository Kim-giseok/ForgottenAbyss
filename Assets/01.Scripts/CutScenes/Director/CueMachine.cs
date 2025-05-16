using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CueMachine
{
    private readonly Queue<Func<UniTask>> _scenario = new();

    public bool isCutSceneStarted = false;
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

    // ReSharper disable once AsyncVoidMethod
    public async void Next()
    {
        if (IsFinish) return;
        
        var cut = _scenario.Dequeue();
        isCutSceneStarted = true;
        await cut.Invoke();
        isCutSceneStarted = false;

        if (_scenario.Count != 0) return;
        IsFinish = true;
        OnFinish?.Invoke();
    }
}