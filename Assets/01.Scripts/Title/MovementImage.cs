using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementImage : MonoBehaviour
{
    public float moveAmount = 10f;
    public float moveSpeed = 2f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float y = Mathf.Sin(Time.time * moveSpeed) * moveAmount;
        transform.localPosition = startPos + new Vector3(0, y, 0);
    }
}
