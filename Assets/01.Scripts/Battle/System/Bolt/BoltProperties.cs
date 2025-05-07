using UnityEngine;

public class BoltProperties : MonoBehaviour
{
    public bool isStarted { get; private set; } = false;
    
    public HitBox hitBox { get; private set; }
    // 공통변수
    public float currTime { get; private set; } = 0f;
    public float currNodeTime { get; private set; } = 0f;
    public Vector3 direction { get; private set; } // 발사체의 방향은 공통 변수로 관리
    
    // Node가 자체적으로 가진다. - 총 합에 해당하는 duration
    public float duration { get; private set; } = 1;
    public float speed { get; private set; } = 10;
}