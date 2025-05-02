using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSimpleMove : MonoBehaviour
{
    [SerializeField] Vector3[] moveDirects;
    [SerializeField] float speed;
    [SerializeField] bool isLoop;

    public void Move()
    {
        if (moveDirects == null) return;
        StartCoroutine(MoveAllRoot());
    }

    IEnumerator MoveAllRoot()
    {
        foreach (var movePoint in moveDirects)
        {
            Vector3 nextP = transform.position + movePoint;
            Vector3 direction = movePoint.normalized;

            while (Vector3.Distance(nextP, transform.position) >= speed * Time.deltaTime)
            {
                transform.position += direction * speed * Time.deltaTime;
                yield return null;
            }
            transform.position = movePoint;
        }
    }
}
