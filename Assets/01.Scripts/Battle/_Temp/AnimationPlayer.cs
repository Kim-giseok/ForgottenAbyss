using System.Collections;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    private Animator animator;
    public string status;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float progress = stateInfo.normalizedTime % 1f;

        if (stateInfo is { normalizedTime: >= 1f, loop: false }) return;

        // 시작 부분인지 확인 (예: 5% 이내)
        if (progress < 0.05f && status != $"start")
        {
            status = "start";
            Debug.Log("start");
        }

        // 끝 부분인지 확인 (예: 마지막 5%)
        if (progress > 0.95f && status != "end")
        {
            status = "end";
            Debug.Log("end");
        }
    }

    private void Start()
    {
        animator.Play("Explosion");
        // StartCoroutine(WaitForAnimation(0.2f)); // 3초 기다리기
    }

    public void checkEvent(int i)
    {
        Debug.Log("event with play");
    }

    private IEnumerator WaitForAnimation(float duration)
    {
        yield return new WaitForSeconds(duration);
        animator.Play("Idle");
        
    }
}