public class EnemyStat
{
    public EnemyStatType statType { get; private set; }
    public float maxValue { get; private set; }
    public float value { get; private set; }

    public EnemyStat(EnemyStatType statType, float value)
    {
        this.statType = statType;
        maxValue = value;
        this.value = value;
    }

    public void Modify(float amount)
    {
        value += amount;
    }

    public void Set(float newValue)
    {
        maxValue = newValue;
        value = newValue;
    }
}