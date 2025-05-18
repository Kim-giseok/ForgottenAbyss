using UnityEngine;

public class CutUIPool: MonoBehaviour
{
    public void Add(RectTransform newUIComp)
    {
        newUIComp.transform.SetParent(transform);
        newUIComp.anchoredPosition = Vector3.zero;
    }

    public void Delete()
    {
        
    }
}