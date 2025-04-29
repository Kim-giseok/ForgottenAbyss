using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragItemPool : MonoBehaviour
{
    public static DragItemPool Instance { get; private set; }

    [SerializeField] private GameObject dragItemPrefab; //원본 프리펩
    [SerializeField] private int initialPoolSize = 10;

    private Queue<GameObject> pool = new Queue<GameObject>(); // 복사본 큐에 저장

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        InitializePool(); 
    }

    private void InitializePool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewItem(); // 복사본 하나 만들고 풀에 추가
        }
    }

    private GameObject CreateNewItem()
    {
        GameObject obj = Instantiate(dragItemPrefab, transform); // 프리펩 복제
        obj.SetActive(false);
        pool.Enqueue(obj); // 큐에 넣기
        return obj;
    }

    public GameObject Get()
    {
        if (pool.Count > 0)
        {
            var obj = pool.Dequeue(); // 맨 앞에 있는 오브젝트
            obj.SetActive(true);
            return obj;
        }
        else
        {
            return CreateNewItem();
        }
    }

    public void Return(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
