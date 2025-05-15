public class EnemyLevelSystem
{ 
    public static int CurrLevel { get; private set; } = 1;
    public static void SetLevel(int newLevel) => CurrLevel = newLevel;
}