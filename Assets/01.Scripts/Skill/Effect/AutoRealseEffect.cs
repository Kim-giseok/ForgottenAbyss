using UnityEngine;

public class AutoReleaseEffect : MonoBehaviour
{
    public string poolKey;
    public float lifeTime = 1.5f;

    private void OnEnable()
    {
        Invoke(nameof(Release), lifeTime);
    }

    public void Release()
    {
        EffectPool.Instance.ReleaseEffect(poolKey, gameObject);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
