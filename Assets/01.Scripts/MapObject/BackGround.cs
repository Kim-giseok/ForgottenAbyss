using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    public Vector3 position
    {
        get => transform.position;
        set
        {
            transform.position = value;
            float gap = right.transform.position.x - left.transform.position.x;
            if (Camera.main.transform.position.x < left.transform.position.x)
            {
                right.transform.position = left.transform.position;
                left.transform.position += Vector3.left * gap;
            }
            else if (Camera.main.transform.position.x > right.transform.position.x)
            {
                left.transform.position = right.transform.position;
                right.transform.position += Vector3.right * gap;
            }
        }
    }

    public int sortingOrder
    {
        get => right.sortingOrder;
        set => right.sortingOrder = left.sortingOrder = value;
    }

    public Sprite sprite
    {
        get => right.sprite;
        set => right.sprite = left.sprite = value;
    }

    public SpriteRenderer right, left;
}
