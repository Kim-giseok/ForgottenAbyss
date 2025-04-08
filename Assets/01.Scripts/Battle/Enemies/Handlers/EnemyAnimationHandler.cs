using UnityEngine;

public class EnemyAnimationHandler
{
    private Animator animator;
    
    public static readonly int Run = Animator.StringToHash("Run");
    public static readonly int Attack = Animator.StringToHash("Attack");
    public static readonly int Hit = Animator.StringToHash("Hit");
    
    public EnemyAnimationHandler(Animator animator) { this.animator = animator; }

    public void Set(int triggerNameHash)
    {
        animator.SetTrigger(triggerNameHash);
    }
    
    public void Set<T>(int triggerNameHash, T value)
    {
        switch (value)
        {
            case bool boolValue: animator.SetBool(triggerNameHash, boolValue); break;
            case int intValue: animator.SetInteger(triggerNameHash, intValue); break;
            case float floatValue: animator.SetFloat(triggerNameHash, floatValue); break;
            default: Debug.LogError("animation handler : fail to change animator trigger"); break;
        }
    }
}