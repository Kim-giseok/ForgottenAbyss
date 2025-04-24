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

    // 스탯 포인트 관련 변수
    [SerializeField] private int availableStatPoints; // 사용 가능한 스탯 포인트
    [SerializeField] private int statPointsPerLevel; // 레벨업 시 획득하는 스탯 포인트

    // 스탯별 투자 가능 최대치
    private Dictionary<StatType, int> maxStatInvestment = new Dictionary<StatType, int>();
    // 스탯별 투자된 포인트
    private Dictionary<StatType, int> investedStatPoints = new Dictionary<StatType, int>();
    // 스탯 포인트당 스탯 증가량
    private Dictionary<StatType, float> statPointIncrease = new Dictionary<StatType, float>();

    private int maxLevel = 999;

    // 스탯 포인트 변경 이벤트
    public delegate void StatPointsChangedHandler(int points);
    public event StatPointsChangedHandler OnStatPointsChanged;

    private StatType[] investableStats = new StatType[]
    {
        StatType.ATK,
        StatType.CRITICAL,
        StatType.MaxHP,
        StatType.DEF,
        StatType.SPEED
    };

    [SerializeField] private GameObject[] StatButtons; // 스탯을 찍는 버튼들
    
    private void Awake()
    {
        InitializeStats();
        InitializeLevelStats();
        InitializeExpRequired();
        InitializeStatPointSystem();
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

        // 테스트용 스탯 포인트 투자
        if (Input.GetKeyDown(KeyCode.Q)) // ATK에 스탯 포인트 투자
        {
            InvestStatPoint(StatType.ATK);
        }
        if (Input.GetKeyDown(KeyCode.W)) // CRITICAL에 스탯 포인트 투자
        {
            InvestStatPoint(StatType.CRITICAL);
        }
        if (Input.GetKeyDown(KeyCode.E)) // MaxHP에 스탯 포인트 투자
        {
            InvestStatPoint(StatType.MaxHP);
        }
        if (Input.GetKeyDown(KeyCode.R)) // DEF에 스탯 포인트 투자
        {
            InvestStatPoint(StatType.DEF);
        }
        if (Input.GetKeyDown(KeyCode.T)) // SPEED에 스탯 포인트 투자
        {
            InvestStatPoint(StatType.SPEED);
        }
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
        stats[StatType.CRITICAL] = 10f; //초기 치명타 확률(%)
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
    // 스탯 포인트 시스템 초기화
    private void InitializeStatPointSystem()
    {
        // 각 스탯별 투자된 포인트 초기화
        foreach (StatType statType in investableStats)
        {
            investedStatPoints[statType] = 0;
        }

        // 스탯 포인트당 증가량 설정
        statPointIncrease[StatType.ATK] = 1f;       // 공격력 증가량
        statPointIncrease[StatType.CRITICAL] = 1f;  // 치명타 확률 증가량
        statPointIncrease[StatType.MaxHP] = 10f;    // 최대 체력 증가량
        statPointIncrease[StatType.DEF] = 1f;       // 방어력 증가량
        statPointIncrease[StatType.SPEED] = 0.2f;   // 이동속도 증가량

        // 최대 투자 가능 포인트 설정
        maxStatInvestment[StatType.ATK] = 10;      // 최대 ATK 투자 포인트
        maxStatInvestment[StatType.CRITICAL] = 10;  // 최대 CRITICAL 투자 포인트 (최대 50% 추가)
        maxStatInvestment[StatType.MaxHP] = 10;    // 최대 MaxHP 투자 포인트
        maxStatInvestment[StatType.DEF] = 10;      // 최대 DEF 투자 포인트
        maxStatInvestment[StatType.SPEED] = 10;     // 최대 SPEED 투자 포인트
    }

    // 스탯 이름 반환 (UI 표시용)
    //public string GetStatName(StatType statType)
    //{
    //    if (statNames.ContainsKey(statType))
    //    {
    //        return statNames[statType];
    //    }
    //    // 기본값으로 스탯 타입 이름 사용
    //    return statType.ToString();
    //}

    // 스탯 포인트 투자 시 증가량 반환
    public float GetStatIncreasePerPoint(StatType statType)
    {
        if (statPointIncrease.ContainsKey(statType))
        {
            return statPointIncrease[statType];
        }
        return 0f;
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

        // 스탯 포인트 추가
        AddStatPoints(statPointsPerLevel);

        Debug.Log($"투자 가능 포인트: {availableStatPoints}");

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

    // 스탯 포인트 추가    
    public void AddStatPoints(int points)
    {
        availableStatPoints += points;
        OnStatPointsChanged?.Invoke(availableStatPoints);
    }

    // 스탯 포인트 투자
    public bool InvestStatPoint(StatType statType)
    {
        // 투자 가능한 스탯인지 확인
        bool isInvestable = false;
        foreach (StatType type in investableStats)
        {
            if (type == statType)
            {
                isInvestable = true;
                break;
            }
        }
               
        // 사용 가능한 스탯 포인트가 있는지 확인
        if (availableStatPoints <= 0)
        {
            Debug.LogWarning("사용 가능한 스탯 포인트가 없습니다.");
            return false;
        }

        // 최대 투자 가능 포인트를 초과하는지 확인
        if (investedStatPoints[statType] >= maxStatInvestment[statType])
        {
            Debug.LogWarning($"{statType}에 더 이상 스탯 포인트를 투자할 수 없습니다. (최대: {maxStatInvestment[statType]})");
            return false;
        }

        // 스탯 포인트 사용
        availableStatPoints--;
        investedStatPoints[statType]++;

        // 스탯 증가 적용
        float newValue = stats[statType] + statPointIncrease[statType];
        SetStat(statType, newValue);

        // 현재 체력도 함께 증가 (MaxHP 포인트 투자시)
        if (statType == StatType.MaxHP)
        {
            float currentHP = stats[StatType.CurrentHP];
            float increase = statPointIncrease[StatType.MaxHP];
            SetStat(StatType.CurrentHP, currentHP + increase);
        }

        // 스탯 포인트 변경 이벤트 발생
        OnStatPointsChanged?.Invoke(availableStatPoints);

        Debug.Log($"{statType}에 스탯 포인트를 투자했습니다. ({statType}: +{statPointIncrease[statType]}, 총 투자: {investedStatPoints[statType]}/{maxStatInvestment[statType]})");
        Debug.Log($"남은 스탯 포인트: {availableStatPoints}");

        return true;
    }

    // 스탯 포인트 리셋 (모든 투자 취소)
    public void ResetStatPoints()
    {
        int totalPoints = 0;

        // 각 스탯 원래 값으로 복원 및 투자 포인트 회수
        foreach (StatType statType in investableStats)
        {
            if (investedStatPoints.ContainsKey(statType))
            {
                // 투자된 포인트 회수
                int pointsInvested = investedStatPoints[statType];
                totalPoints += pointsInvested;

                // 원래 스탯 값 계산 (현재 값 - 투자로 인한 증가량)
                float originalValue = stats[statType] - (pointsInvested * statPointIncrease[statType]);
                SetStat(statType, originalValue);

                // 투자된 포인트 초기화
                investedStatPoints[statType] = 0;
            }
        }

        // MaxHP 변경 시 CurrentHP도 조정
        if (investedStatPoints.ContainsKey(StatType.MaxHP))
        {
            float currentHPRatio = stats[StatType.CurrentHP] / stats[StatType.MaxHP];
            SetStat(StatType.CurrentHP, stats[StatType.MaxHP] * currentHPRatio);
        }

        // 사용 가능한 스탯 포인트 복원
        availableStatPoints += totalPoints;
        OnStatPointsChanged?.Invoke(availableStatPoints);

        Debug.Log($"모든 스탯 포인트가 초기화되었습니다. (사용 가능한 포인트: {availableStatPoints})");
    }

    // 현재 사용 가능한 스탯 포인트를 반환
    public int GetAvailableStatPoints()
    {
        return availableStatPoints;
    }

    // 특정 스탯에 투자된 포인트를 반환
    public int GetInvestedStatPoints(StatType statType)
    {
        if (investedStatPoints.ContainsKey(statType))
        {
            return investedStatPoints[statType];
        }
        return 0;
    }

    // 특정 스탯의 최대 투자 가능 포인트를 반환
    public int GetMaxStatInvestment(StatType statType)
    {
        if (maxStatInvestment.ContainsKey(statType))
        {
            return maxStatInvestment[statType];
        }
        return 0;
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
