using System;
using UnityEngine;


// 모노싱글톤
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType(typeof(T)) as T;

            if (_instance == null)
                _instance = Instantiate(Resources.Load<T>("Prefabs/Managers/"+ typeof(T)));

            if (_instance == null)
                _instance = new GameObject($"{typeof(T)}").AddComponent<T>();

            return _instance;
        }
    }
    protected static T _instance = null; // 실제 인스턴스 핸들

    protected virtual void Awake()
    {
        if (_instance == null)
            _instance = this as T;
        else if (_instance != this as T)
            Destroy(gameObject);
    }

    protected virtual void Init() { }

    protected virtual void OnDestroy()
    {
        if (_instance != null && _instance == this as T)
            _instance = null;
    }
}
