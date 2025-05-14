using UnityEngine;

public class DragItemPool : MonoBehaviour
{
    [SerializeField] private GameObject dragItemPrefab; //원본 프리펩
    [SerializeField] private int poolSize = 10;

    private GameObject[] pool;
    private int nextIndex = 0;

    private void Awake()
    {
        Debug.Log("[DragItemPool] Awake 실행됨");

        if (dragItemPrefab == null)
        {
            Debug.LogError("[DragItemPool] dragItemPrefab이 null입니다! 드래그 아이템 프리팹을 연결하세요.");
            return;
        }

        pool = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            pool[i] = Instantiate(dragItemPrefab, transform);
            pool[i].SetActive(false);
        }
    }

    public GameObject Get()
    {
        for (int i = 0; i < poolSize; i++)
        {
            int index = (nextIndex + i) % poolSize;
            if (!pool[index].activeInHierarchy)
            {
                nextIndex = (index + 1) % poolSize;
                pool[index].SetActive(true);
                return pool[index];
            }
        }

        // 모두 사용 중이면 새로 생성
        var extra = Instantiate(dragItemPrefab, transform);
        extra.SetActive(true);
        return extra;
    }

    public void Return(GameObject obj)
    {
        if (obj == null)
        {
            Debug.LogWarning("[DragItemPool] 반환하려는 오브젝트가 null입니다.");
            return;
        }

        obj.SetActive(false);
    }
}
