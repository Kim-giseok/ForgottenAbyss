using UnityEngine;

public class PointingComp : MonoBehaviour
{
    private  RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void SetSize(Transform target)
    {
        transform.localScale = target.localScale * 2.5f;
    }

    public void On(bool isOn)
    {
        transform.position = Vector3.zero;
        gameObject.SetActive(isOn);
    }
}