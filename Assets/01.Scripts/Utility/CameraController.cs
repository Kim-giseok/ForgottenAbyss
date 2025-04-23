using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player { get; private set; }

    public int depth;

    private float originFOV;
    public float zoomedFOV = 80f;
    public float zoomDuration = 30f;
    public float holdTime = 30f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (Camera.main) originFOV = Camera.main.fieldOfView;
    }
    
    public void FixedUpdate()
    {
        transform.position = new Vector3(player.position.x, player.position.y, -depth);
    }

    // private void Start()
    // {
    //     TriggerZoomEffect();
    // }
    
    public void TriggerZoomEffect()
    {
        StopAllCoroutines();
        StartCoroutine(ZoomEffectRoutine());
    }
    
    private IEnumerator ZoomEffectRoutine()
    {
        // 확대
        yield return StartCoroutine(LerpFOV(originFOV, zoomedFOV, zoomDuration));

        // 잠깐 유지
        yield return new WaitForSeconds(holdTime);

        // 원래대로
        yield return StartCoroutine(LerpFOV(zoomedFOV, originFOV, zoomDuration));
    }

    private IEnumerator LerpFOV(float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            Camera.main.fieldOfView = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }
        Camera.main.fieldOfView = to;
    }
}
