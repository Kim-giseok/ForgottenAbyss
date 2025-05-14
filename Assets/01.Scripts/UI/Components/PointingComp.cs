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
        float worldScale = target.lossyScale.magnitude;
        _rectTransform.sizeDelta = new Vector2(100, 100) * worldScale * 1.4f;
    }

    public void On(bool isOn)
    {
        transform.position = Vector3.zero;
        gameObject.SetActive(isOn);
    }
}