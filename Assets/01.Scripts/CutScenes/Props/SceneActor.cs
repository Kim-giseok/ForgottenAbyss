using UnityEngine;

public class SceneActor: MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rigid;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
    }
}