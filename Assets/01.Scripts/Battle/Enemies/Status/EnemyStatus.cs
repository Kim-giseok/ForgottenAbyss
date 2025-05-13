
public enum StatusEffect { Slow, Stun, Freeze, Faint }

public abstract class EnemyStatus
{
    public StatusEffect statusEffect { get; private set; }
    public float duration;
    public float currTime;

    public void Start()
    {
        
    }

    public void Update()
    {
        
    }

    public void End()
    {
        
    }
}