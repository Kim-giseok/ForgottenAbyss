using UnityEngine;

public class CutUIPool: MonoBehaviour
{
    public void Set(RectTransform newUIComp, Vector3 newPos = default)
    {
        if(newUIComp.parent != transform) { newUIComp.transform.SetParent(transform); }
        newUIComp.anchoredPosition = newPos == default ? Vector3.zero : newPos;
    }

    public void Delete()
    {
        
    }
}