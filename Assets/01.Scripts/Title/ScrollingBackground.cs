using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ScrollingBackground : MonoBehaviour
{
    [System.Serializable]
    public class ScrollingLayer
    {
        public Transform[] layerInstances;
        public float moveSpeed;
        private float spriteWidth;

        public void Initialize()
        {
            Tilemap tilemap = layerInstances[0].GetComponent<Tilemap>();
            if (tilemap != null)
            {
                spriteWidth = tilemap.localBounds.size.x;
            }
            else
            {
                Renderer renderer = layerInstances[0].GetComponent<Renderer>();
                if (renderer != null)
                {
                    spriteWidth = renderer.bounds.size.x;
                }
            }
        }

        public void UpdateLayer(float deltaTime)
        {
            foreach (var t in layerInstances)
            {
                if (t != null)
                    t.position += Vector3.left * moveSpeed * deltaTime;
            }

            for (int i = 0; i < layerInstances.Length; i++)
            {
                Transform t = layerInstances[i];
                if (t == null) continue;

                if (t.position.x < Camera.main.transform.position.x - spriteWidth)
                {
                    Transform rightMost = GetRightMost();
                    if (rightMost != null)
                    {
                        t.position = new Vector3(
                            rightMost.position.x + spriteWidth,
                            t.position.y,
                            t.position.z
                        );
                    }
                }
            }
        }

        private Transform GetRightMost()
        {
            Transform rightMost = layerInstances[0];
            foreach (var t in layerInstances)
            {
                if (t != null && t.position.x > rightMost.position.x)
                    rightMost = t;
            }
            return rightMost;
        }
    }

    public ScrollingLayer[] layers;

    void Start()
    {
        foreach (var layer in layers)
        {
            layer.Initialize();
        }
    }

    void Update()
    {
        float dt = Time.deltaTime;
        foreach (var layer in layers)
        {
            layer.UpdateLayer(dt);
        }
    }
}
