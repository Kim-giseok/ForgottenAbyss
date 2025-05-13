
public enum StatusEffect { Slow, Stun, Freeze, Faint }

public abstract class EnemyStatus
{
    public virtual StatusEffect statusEffect { get; private set; }
    public float duration;
    public float currTime;

    public virtual void Start()
    {
        
    }

    public virtual void Update()
    {
        
    }

    public virtual void End()
    {
        
    }
}