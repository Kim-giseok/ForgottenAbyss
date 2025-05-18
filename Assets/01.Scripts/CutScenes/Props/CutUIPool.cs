using UnityEngine;

public class CutUIPool: MonoBehaviour
{
    public void Add(GameObject newUIComp)
    {
        newUIComp.transform.SetParent(transform);
        newUIComp.transform.position = Vector3.zero;
    }

    public void Delete()
    {
        
    }
}