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

    float gap => right.transform.position.x - left.transform.position.x;

    public int sortingOrder
    {
        get => right.sortingOrder;
        set => right.sortingOrder = left.sortingOrder = value;
    }

    public Sprite sprite
    {
        get => right.sprite;
        set
        {
            right.sprite = left.sprite = value;
            right.transform.localPosition = new Vector3(sprite.bounds.extents.x, 0);
            left.transform.localPosition = new Vector3(-sprite.bounds.extents.x, 0);

            transform.localScale = Vector2.one * (Camera.main.orthographicSize / sprite.bounds.extents.y);
        }
    }

    public SpriteRenderer right, left;
}
