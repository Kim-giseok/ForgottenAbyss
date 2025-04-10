using UnityEngine;

public class TempTester: MonoBehaviour
{
    private Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Start()
    {
        rigidbody.AddForce(new Vector2(1f, 1f), ForceMode2D.Impulse); // 탱탱볼처럼 튕기지 않음
    }
}