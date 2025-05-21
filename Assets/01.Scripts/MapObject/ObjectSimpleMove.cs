using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectSimpleMove : MonoBehaviour
{
    [SerializeField] Vector3[] moveDirects;
    [SerializeField] float speed;
    [SerializeField] bool isLoop;
    Vector3 originP;

    public void Move()
    {
        if (originP == null)
            originP = transform.position;
        if (moveDirects == null) return;
        StartCoroutine(MoveAllRoot());
    }

    IEnumerator MoveAllRoot()
    {
        Vector3 nextP, direction;
        while (true)
        {
            foreach (var movePoint in moveDirects)
            {
                nextP = transform.position + movePoint;
                direction = movePoint.normalized;

                while (Vector3.Distance(nextP, transform.position) >= speed * Time.deltaTime)
                {
                    transform.position += direction * speed * Time.deltaTime;
                    yield return null;
                }
                transform.position = nextP;
            }
            if (!isLoop) break;

            nextP = originP;
            direction = (originP - transform.position).normalized;

            while (Vector3.Distance(nextP, transform.position) >= speed * Time.deltaTime)
            {
                transform.position += direction * speed * Time.deltaTime;
                yield return null;
            }
            transform.position = nextP;
        }
    }

    private void OnDrawGizmos()
    {
        if (moveDirects == null || moveDirects.Length == 0) return;

        Gizmos.color = Color.red;
        Vector3 startP = originP == null ? transform.position : originP;

        foreach (var movedirect in moveDirects)
        {
            Vector3 nextP = startP + movedirect;
            Gizmos.DrawLine(startP, nextP);
            startP = nextP;
        }

        Gizmos.DrawSphere(startP, 0.2f);
    }
}
