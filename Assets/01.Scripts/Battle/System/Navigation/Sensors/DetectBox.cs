using UnityEngine;

// 전후방 장애물 감지
public class DetectBox: MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌 방향 체크(앞과 뒤에서)
        Vector3 direction = (collision.transform.position - transform.position).normalized;
    }
}