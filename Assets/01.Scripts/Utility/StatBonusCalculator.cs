using System.Collections.Generic;

public static class StatBonusCalculator
{
    public static float ApplyBonuses(float baseValue, List<ArmorStatBonus> bonuses)
    {
        float flatTotal = 0f;
        float percentTotal = 0f;

        foreach (var bonus in bonuses)
        {
            switch (bonus.bonusType)
            {
                case BonusType.Flat:
                    flatTotal += bonus.value;
                    break;
                case BonusType.Percent:
                    percentTotal += bonus.value;
                    break;
            }
        }

        float flatApplied = baseValue + flatTotal;
        float percentApplied = flatApplied * (1 + percentTotal / 100f);
        return percentApplied;
    }

    public static float RemoveBonuses(float currentValue, List<ArmorStatBonus> bonuses)
    {
        float flatTotal = 0f;
        float percentTotal = 0f;

        foreach (var bonus in bonuses)
        {
            switch (bonus.bonusType)
            {
                case BonusType.Flat:
                    flatTotal += bonus.value;
                    break;
                case BonusType.Percent:
                    percentTotal += bonus.value;
                    break;
            }
        }

        float flatRestored = currentValue / (1 + percentTotal / 100f);
        float baseRestored = flatRestored - flatTotal;
        return baseRestored;
    }
}