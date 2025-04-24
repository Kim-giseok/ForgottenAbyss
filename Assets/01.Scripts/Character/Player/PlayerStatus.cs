using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PlayerStatus : CharacterStatus
{
    // 레벨업에 필요한 경험치 요구량
    private Dictionary<int, float> expRequiredForLevel = new Dictionary<int, float>();
    // 레벨별 스탯 증가량
    private Dictionary<int, Dictionary<StatType, float>> levelStats = new Dictionary<int, Dictionary<StatType, float>>();

    private int maxLevel = 999;
    private void Awake()
    {
        InitializeStats();
        InitializeLevelStats();
        InitializeExpRequired();
    }

    private void Start()
    {
        var binder = FindObjectOfType<PlayerUIBinder>();
        if (binder != null)
        {
            binder.BindStatus(this);
        }
    }
    // 이벤트 구독
    private void OnEnable()
    {
        OnStatChanged += (type, value) =>
        {
            if (type == StatType.CurrentHP)
                Debug.Log($"HP 변경됨: {value}");
        };
    }

    private void Update()
    {
        // ! 테스트용 체력감소 !
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            float cur = stats[StatType.CurrentHP];
            SetStat(StatType.CurrentHP, cur - 10f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) // 마나 소모
        {
            float cur = stats[StatType.CurrentMP];
            SetStat(StatType.CurrentMP, Mathf.Max(0, cur - 10f));
        }

        TestExp();
    }
    private void InitializeStats()
    {
        stats[StatType.CurrentHP] = 100f; //현재 HP
        stats[StatType.MaxHP] = 100f; //초기 HP
        stats[StatType.CurrentMP] = 100f; //현재 MP
        stats[StatType.MaxMP] = 100f; //초기 MP
        stats[StatType.ATK] = 10f; //초기 공격력
        stats[StatType.DEF] = 10f; //초기 방어력
        stats[StatType.LEVEL] = 1f; //초기 레벨
        stats[StatType.EXP] = 0f; //초기 경험치
        stats[StatType.MaxEXP] = 0f; //초기 경험치
        stats[StatType.GOLD] = 0f; //초기 골드
        stats[StatType.SPEED] = 3f; //초기 골드
        stats[StatType.CRITICAL] = 50f; //초기 크리티컬 확률
        stats[StatType.CRITICAL_DAMAGE] = 150f; //초기 크리티컬 데미지 비율
        stats[StatType.COOLDOWN_REDUCTION] = 40f; //초기 쿨타임 감소 비율
    }

    // 레벨별 스탯 증가량 초기화
    private void InitializeLevelStats()
    {
        // 레벨별 스탯 증가량 설정 (레벨 2부터 시작)
        for (int level = 2; level <= maxLevel; level++)
        {
            Dictionary<StatType, float> statIncreases = new Dictionary<StatType, float>();

            // 레벨별 증가량
            statIncreases[StatType.MaxHP] = 20f;         // HP 증가량
            statIncreases[StatType.MaxMP] = 15f;        // MP 증가량
            statIncreases[StatType.ATK] = 1f;     // 공격력 증가량
            statIncreases[StatType.DEF] = 1f;     // 방어력 증가량

            levelStats[level] = statIncreases;
        }
    }

    private void InitializeExpRequired()
    {
        // 레벨별 필요 경험치 설정
        for (int level = 1; level <= maxLevel; level++)
        {
            // 경험치 공식 (예: level^2 * 100)
            expRequiredForLevel[level] = level * 100f;
        }

        int currentLevel = (int)stats[StatType.LEVEL];
        stats[StatType.MaxEXP] = expRequiredForLevel[currentLevel];
    }

    // 경험치 획득 메서드
    public void GainExperience(float amount)
    {
        SetStat(StatType.EXP, stats[StatType.EXP] + amount);
        //stats[StatType.EXP] += amount;
        Debug.Log($"경험치 획득: +{amount} (현재: {stats[StatType.EXP]})");

        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        int curLevel = (int)stats[StatType.LEVEL];

        // 최대 레벨에 도달했는지 체크
        if (curLevel >= maxLevel)
        {
            stats[StatType.EXP] = expRequiredForLevel[maxLevel]; // 경험치 제한
            return;
        }

        // 현재 레벨에서 필요한 경험치를 넘었는지 체크
        if (stats[StatType.EXP] >= expRequiredForLevel[curLevel])
        {
            LevelUp();

            // 남은 경험치가 또 레벨업에 충분한지 확인 (연속 레벨업 처리)
            CheckLevelUp();
        }
    }

    private void LevelUp()
    {
        int newLevel = (int)stats[StatType.LEVEL] + 1;

        // 경험치 계산
        stats[StatType.EXP] -= expRequiredForLevel[(int)stats[StatType.LEVEL]];

        // 레벨 증가
        stats[StatType.LEVEL] = newLevel;

        // 레벨에 따른 스탯 증가
        ApplyLevelStats(newLevel);

        stats[StatType.CurrentHP] = stats[StatType.MaxHP];
        stats[StatType.CurrentMP] = stats[StatType.MaxMP];

        int currentLevel = (int)stats[StatType.LEVEL];
        stats[StatType.MaxEXP] = expRequiredForLevel[currentLevel];

        Debug.Log($"현재 레벨: {stats[StatType.LEVEL]}");
        Debug.Log($"HP: {stats[StatType.MaxHP]}");
        Debug.Log($"MP: {stats[StatType.MaxMP]}");
        Debug.Log($"ATK: {stats[StatType.ATK]}");
        Debug.Log($"DEF: {stats[StatType.DEF]}");

    }

    // 레벨에 따른 스탯 적용
    private void ApplyLevelStats(int level)
    {
        if (levelStats.ContainsKey(level))
        {
            Dictionary<StatType, float> statIncreases = levelStats[level];

            foreach (var statType in statIncreases.Keys)
            {
                float newValue = stats[statType] + statIncreases[statType];
                SetStat(statType, newValue); // 이벤트 발생 포함
                //stats[statType] += statIncreases[statType];
                Debug.Log($"{statType} 증가: +{statIncreases[statType]}");
            }
        }
    }

    private void GetGold() //골드 획득 
    {

    }

    private void TestExp() //테스트 경험치 획득
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            GainExperience(50);
        }
    }

    public float GetStat(StatType statType)
    {
        stats.TryGetValue(statType, out float value);
        return value;
    }
}
