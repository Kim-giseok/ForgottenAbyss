public class EnemyStat
{
    public EnemyStatType statType { get; private set; }
    public float maxValue { get; private set; }
    public float currValue { get; private set; }

    public EnemyStat(EnemyStatType statType, float value)
    {
        this.statType = statType;
        maxValue = value;
        currValue = value;
    }

    public void Modify(float amount)
    {
        currValue += amount;
    }

    public void Set(float newValue)
    {
        maxValue = newValue;
        currValue = newValue;
    }
}