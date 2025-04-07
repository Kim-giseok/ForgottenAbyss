using UnityEngine;

public class CameraController : MonoBehaviour
{
    public int depth;
    public Transform player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void FixedUpdate()
    {
        transform.position = new Vector3(player.position.x, player.position.y, -depth);
    }
}
