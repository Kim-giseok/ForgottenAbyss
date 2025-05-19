using UnityEngine;

public class PointingComp : MonoBehaviour
{
    private  RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }
    
    public void Set(Transform target = null)
    {
        if (!target) { gameObject.SetActive(false); return; }
        gameObject.SetActive(true);
        
        SoundManager.Instance.Playsfx("Pointing");
        transform.localScale = target.localScale * 2.5f;
        transform.position = target.position;   
    }
}