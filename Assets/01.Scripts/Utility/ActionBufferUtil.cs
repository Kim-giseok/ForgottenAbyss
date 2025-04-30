using System;
using System.Collections.Generic;
using UnityEngine;

public class ActionBufferUtil : MonoBehaviour
{
    private List<BufferedAction> bufferedActions = new List<BufferedAction>();

    public void BufferAction(string name, Func<bool> condition, Action action)
    {
        // 이름 중복 제거
        bufferedActions.RemoveAll(a => a.Name == name);
        bufferedActions.Add(new BufferedAction(name, condition, action));
    }

    public void Update()
    {
        for (int i = bufferedActions.Count - 1; i >= 0; i--)
        {
            if (bufferedActions[i].TryExecute())
            {
                bufferedActions.RemoveAt(i);
            }
        }
    }

    public void Clear(string name)
    {
        bufferedActions.RemoveAll(a => a.Name == name);
    }

    public void ClearAll()
    {
        bufferedActions.Clear();
    }
}