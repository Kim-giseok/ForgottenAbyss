using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerStatus : CharacterStatus
{
    // 레벨 업 필요 경험치
    private Dictionary<int, float> expRequiredForLevel = new Dictionary<int, float>();
    // 레벨별 스탯
    private Dictionary<int, Dictionary<StatType, float>> levelStats = new Dictionary<int, Dictionary<StatType, float>>();


    [SerializeField] private int availableStatPoints; // 사용 가능한 스탯 포인트
    [SerializeField] private int statPointsPerLevel; // 레벨 업 시 획득하는 스탯 포인트

    // 최대 투자 가능 스탯 포인트
    private Dictionary<StatType, int> maxStatInvestment = new Dictionary<StatType, int>();
    // 투자한 스탯 포인트
    private Dictionary<StatType, int> investedStatPoints = new Dictionary<StatType, int>();
    // 패시브 스탯 증가량
    private Dictionary<StatType, float> statPointIncrease = new Dictionary<StatType, float>();

    private int maxLevel = 999;

    // 스탯 변경 이벤트
    public delegate void StatPointsChangedHandler(int points);
    public event StatPointsChangedHandler OnStatPointsChanged;

    public PassiveUI passiveUI;

    private const string PLAYER_DATA_FILE = "player_status.json";

    private StatType[] investableStats = new StatType[]
    {
        StatType.ATK,
        StatType.CRITICAL,
        StatType.MaxHP,
        StatType.DEF,
        StatType.SPEED
    };

    public void SavePlayerData()
    {
        PlayerData data = new PlayerData();

        // 스탯 정보 저장
        foreach (var pair in stats)
        {
            data.stats.Add(new PlayerData.StatData
            {
                statType = (int)pair.Key,
                value = pair.Value
            });
        }

        // 스탯 포인트 저장
        data.availableStatPoints = availableStatPoints;

        // 투자된 스탯 포인트 저장
        foreach (var pair in investedStatPoints)
        {
            data.investedStats.Add(new PlayerData.InvestedStatData
            {
                statType = (int)pair.Key,
                points = pair.Value
            });
        }

        foreach (var pair in equipmentBonuses)
        {
            data.equipmentBonuses.Add(new PlayerData.StatBonusData
            {
                statType = (int)pair.Key,
                bonusValue = pair.Value
            });
        }

        foreach (var pair in setBonusMultipliers)
        {
            data.setBonusMultipliers.Add(new PlayerData.StatMultiplierData
            {
                statType = (int)pair.Key,
                multiplier = pair.Value
            });
        }

        foreach (var id in equippedArmorIDs)
        {
            data.equippedArmorIDs.Add(id);
        }

        // DataSave 클래스를 사용하여 저장
        DataSave<PlayerData>.SaveData(data, PLAYER_DATA_FILE);
    }

    // 데이터 로드 메서드
    public void LoadPlayerData()
    {
        PlayerData data = DataSave<PlayerData>.LoadData(PLAYER_DATA_FILE);

        if (data != null)
        {
            // 스탯 설정
            foreach (var statData in data.stats)
            {
                StatType type = (StatType)statData.statType;
                SetStat(type, statData.value);
            }

            // 스탯 포인트 설정
            availableStatPoints = data.availableStatPoints;

            // 투자된 스탯 포인트 설정
            foreach (var investedData in data.investedStats)
            {
                StatType type = (StatType)investedData.statType;
                if (investedStatPoints.ContainsKey(type))
                {
                    investedStatPoints[type] = investedData.points;
                }
            }

            equipmentBonuses.Clear();
            foreach (var bonusData in data.equipmentBonuses)
            {
                StatType type = (StatType)bonusData.statType;
                equipmentBonuses[type] = bonusData.bonusValue;
            }

            setBonusMultipliers.Clear();
            foreach (var multiplierData in data.setBonusMultipliers)
            {
                StatType type = (StatType)multiplierData.statType;
                setBonusMultipliers[type] = multiplierData.multiplier;
            }

            equippedArmorIDs.Clear();

            // UI 업데이트
            OnStatPointsChanged?.Invoke(availableStatPoints);

            Debug.Log("플레이어 데이터 로드 완료");
        }
    }

    // 씬 전환 전 호출 (OnDisable 또는 OnDestroy)
    private void OnDisable()
    {
        SavePlayerData();
    }

    // 데이터 초기화 메서드
    public void ResetPlayerData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, PLAYER_DATA_FILE);

        // 저장 파일이 존재하면 삭제
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("플레이어 데이터 파일 삭제됨: " + filePath);
        }

        // 현재 실행 중인 게임의 데이터도 초기화
        InitializeStats();
        InitializeStatPointSystem();

        // UI 업데이트
        OnStatPointsChanged?.Invoke(availableStatPoints);
    }

    public void OnApplicationQuit()
    {
        ResetPlayerData();
    }

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
        LoadPlayerData();
    }

    private void OnEnable()
    {
        OnStatChanged += (type, value) =>
        {
            if (type == StatType.CurrentHP)
                Debug.Log($"HP �����: {value}");
        };
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            GainExperience(100f);
        }
    }

    private void InitializeStats()
    {
        stats[StatType.CurrentHP] = 100f; //현재 HP
        stats[StatType.MaxHP] = 100f; //최대 HP
        stats[StatType.CurrentMP] = 100f; //현재 MP
        stats[StatType.MaxMP] = 100f; //최대 MP
        stats[StatType.ATK] = 10f; //초기 공격력
        stats[StatType.DEF] = 10f; //초기 방어력
        stats[StatType.LEVEL] = 1f; //초기 레벨
        stats[StatType.EXP] = 0f; //초기 경험치
        stats[StatType.MaxEXP] = 100f; //레벨 업 경험치
        stats[StatType.GOLD] = 0f; //초기 골드
        stats[StatType.SPEED] = 3f; //이동속도
        stats[StatType.CRITICAL] = 5f; //치명타 확률
        stats[StatType.CRITICAL_DAMAGE] = 110f; //치명타 데미지
        stats[StatType.COOLDOWN_REDUCTION] = 0f; //스킬 쿨타임 감소
    }


    private void InitializeLevelStats()
    {

        for (int level = 2; level <= maxLevel; level++)
        {
            Dictionary<StatType, float> statIncreases = new Dictionary<StatType, float>();

            // 레벨 업 스탯 증가량
            statIncreases[StatType.MaxHP] = 20f;         // HP 증가량
            statIncreases[StatType.MaxMP] = 10f;        // MP 증가량
            statIncreases[StatType.ATK] = 1f;     // 공격력 증가량
            statIncreases[StatType.DEF] = 1f;     // 방어력 증가량

            levelStats[level] = statIncreases;
        }
    }

    private void InitializeExpRequired()
    {

        for (int level = 1; level <= maxLevel; level++)
        {
            // 레벨 업 필요 경험치
            expRequiredForLevel[level] = level * 100f;
        }

        int currentLevel = (int)stats[StatType.LEVEL];

        baseStats[StatType.MaxEXP] = expRequiredForLevel[currentLevel];
        stats[StatType.MaxEXP] = expRequiredForLevel[currentLevel];
    }

    private void InitializeStatPointSystem()
    {

        foreach (StatType statType in investableStats)
        {
            investedStatPoints[statType] = 0;
        }

        // 패시브 스탯 증가량
        statPointIncrease[StatType.ATK] = 1f;       // 공격력 증가량
        statPointIncrease[StatType.CRITICAL] = 1f;  // 치명타 확률 증가량
        statPointIncrease[StatType.MaxHP] = 10f;    // 최대HP 증가량
        statPointIncrease[StatType.DEF] = 1f;       // 방어력 증가량
        statPointIncrease[StatType.SPEED] = 0.2f;   // 이동속도 증가량

        // 패시브 스탯 최대 레벨
        maxStatInvestment[StatType.ATK] = 10;
        maxStatInvestment[StatType.CRITICAL] = 10;
        maxStatInvestment[StatType.MaxHP] = 10;
        maxStatInvestment[StatType.DEF] = 10;
        maxStatInvestment[StatType.SPEED] = 10;
    }



    public float GetStatIncreasePerPoint(StatType statType)
    {
        if (statPointIncrease.ContainsKey(statType))
        {
            return statPointIncrease[statType];
        }
        return 0f;
    }


    public void GainExperience(float amount)
    {
        SetStat(StatType.EXP, stats[StatType.EXP] + amount);

        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        int curLevel = (int)stats[StatType.LEVEL];


        if (curLevel >= maxLevel)
        {
            stats[StatType.EXP] = expRequiredForLevel[maxLevel];
            return;
        }

        // 현재 경험치가 필요 경험치 이상인 경우 레벨 업
        if (stats[StatType.EXP] >= expRequiredForLevel[curLevel])
        {
            BoltsPool.Instance.CreateParticle(transform, "LevelUp").SetDegree(0).SetTrail(false)
                .SetSize(1.2f).SetDuration(1f).SetPosition(transform.position + new Vector3(0f, 1.5f, 0f)).Play();
            SoundManager.Instance.Playsfx("LevelUp");

            LevelUp();

            CheckLevelUp();
        }
    }

    private void LevelUp()
    {
        int newExp = (int)stats[StatType.LEVEL];
        int newLevel = (int)stats[StatType.LEVEL] + 1;

        baseStats[StatType.EXP] = stats[StatType.EXP] - expRequiredForLevel[newExp];
        SetStat(StatType.EXP, stats[StatType.EXP] - expRequiredForLevel[newExp]);

        baseStats[StatType.LEVEL] = newLevel;
        SetStat(StatType.LEVEL, newLevel);


        ApplyLevelStats(newLevel);


        AddStatPoints(statPointsPerLevel);

        Debug.Log($"스탯 포인트: {availableStatPoints}");

        if (passiveUI != null)
        {
            passiveUI.UpdateStatPointsUI(availableStatPoints);
        }

        SetStat(StatType.CurrentHP, stats[StatType.MaxHP]);
        SetStat(StatType.CurrentMP, stats[StatType.MaxMP]);

        int currentLevel = (int)stats[StatType.LEVEL];
        SetStat(StatType.MaxEXP, expRequiredForLevel[currentLevel]);

        Debug.Log($"���� ����: {stats[StatType.LEVEL]}");
        Debug.Log($"HP: {stats[StatType.MaxHP]}");
        Debug.Log($"MP: {stats[StatType.MaxMP]}");
        Debug.Log($"ATK: {stats[StatType.ATK]}");
        Debug.Log($"DEF: {stats[StatType.DEF]}");
    }


    private void ApplyLevelStats(int level)
    {
        if (levelStats.ContainsKey(level))
        {
            Dictionary<StatType, float> statIncreases = levelStats[level];

            foreach (var statType in statIncreases.Keys)
            {
                baseStats[statType] += statIncreases[statType];
                SetStat(statType, baseStats[statType]);
            }
            SystemManager.Instance.equipmentManager.RecalculateStats();
        }
    }


    public void AddStatPoints(int points)
    {
        availableStatPoints += points;
        OnStatPointsChanged?.Invoke(availableStatPoints);
    }


    public bool InvestStatPoint(StatType statType)
    {
        if (!stats.ContainsKey(statType) || availableStatPoints <= 0) return false;

        if (investedStatPoints[statType] >= maxStatInvestment[statType]) return false;

        // 패시브 스탯 투자 시
        availableStatPoints--;
        investedStatPoints[statType]++;

        baseStats[statType] += statPointIncrease[statType];
        SetStat(statType, baseStats[statType]);
        Debug.Log(baseStats[statType]);

        SystemManager.Instance.equipmentManager.RecalculateStats();

        // 이벤트 구독
        OnStatPointsChanged?.Invoke(availableStatPoints);

        Debug.Log($"{statType} 스탯 포인트가 투자되었습니다. ({statType}: +{statPointIncrease[statType]}, 현재 투자: {investedStatPoints[statType]}/{maxStatInvestment[statType]})");
        Debug.Log($"남은 스탯 포인트: {availableStatPoints}");

        return true;
    }

    // 패시브 스탯 초기화
    public void ResetStatPoints()
    {
        int totalPoints = 0;

        foreach (StatType statType in investableStats)
        {
            if (investedStatPoints.ContainsKey(statType))
            {

                int pointsInvested = investedStatPoints[statType];
                totalPoints += pointsInvested;

                float originalValue = stats[statType] - (pointsInvested * statPointIncrease[statType]);
                SetStat(statType, originalValue);

                investedStatPoints[statType] = 0;
            }
        }

        if (investedStatPoints.ContainsKey(StatType.MaxHP))
        {
            float currentHPRatio = stats[StatType.CurrentHP] / stats[StatType.MaxHP];
            SetStat(StatType.CurrentHP, stats[StatType.MaxHP] * currentHPRatio);
        }

        availableStatPoints += totalPoints;
        OnStatPointsChanged?.Invoke(availableStatPoints);
    }

    // 패시브 스탯 포인트 획득
    public int GetAvailableStatPoints()
    {
        return availableStatPoints;
    }


    public int GetInvestedStatPoints(StatType statType)
    {
        if (investedStatPoints.ContainsKey(statType))
        {
            return investedStatPoints[statType];
        }
        return 0;
    }


    public int GetMaxStatInvestment(StatType statType)
    {
        if (maxStatInvestment.ContainsKey(statType))
        {
            return maxStatInvestment[statType];
        }
        return 0;
    }
    private void GetGold()
    {

    }


    public float GetStat(StatType statType)
    {
        stats.TryGetValue(statType, out float value);
        return value;
    }
}
