using System;
using UnityEngine;


// 모노싱글톤
public class Singleton<T> : MonoBehaviour, IDisposable where T : class
{
    protected Singleton()
    {
        Init();
    }

    public static T Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType(typeof(T)) as T;

            return _instance;
        }
    }
    protected static T _instance = null; // 실제 인스턴스 핸들
    protected virtual void Init() { }

    public void Dispose() { }
}
