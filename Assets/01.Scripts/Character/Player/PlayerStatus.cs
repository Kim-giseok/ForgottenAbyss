using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PlayerStatus : CharacterStatus
{
    // �������� �ʿ��� ����ġ �䱸��
    private Dictionary<int, float> expRequiredForLevel = new Dictionary<int, float>();
    // ������ ���� ������
    private Dictionary<int, Dictionary<StatType, float>> levelStats = new Dictionary<int, Dictionary<StatType, float>>();

    // ���� ����Ʈ ���� ����
    [SerializeField] private int availableStatPoints; // ��� ������ ���� ����Ʈ
    [SerializeField] private int statPointsPerLevel; // ������ �� ȹ���ϴ� ���� ����Ʈ

    // ���Ⱥ� ���� ���� �ִ�ġ
    private Dictionary<StatType, int> maxStatInvestment = new Dictionary<StatType, int>();
    // ���Ⱥ� ���ڵ� ����Ʈ
    private Dictionary<StatType, int> investedStatPoints = new Dictionary<StatType, int>();
    // ���� ����Ʈ�� ���� ������
    private Dictionary<StatType, float> statPointIncrease = new Dictionary<StatType, float>();

    private int maxLevel = 999;

    // ���� ����Ʈ ���� �̺�Ʈ
    public delegate void StatPointsChangedHandler(int points);
    public event StatPointsChangedHandler OnStatPointsChanged;

    PassiveUI passiveUI;

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
        passiveUI = FindObjectOfType<PassiveUI>();
       
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
    // �̺�Ʈ ����
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
        // ! �׽�Ʈ�� ü�°��� !
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            float cur = stats[StatType.CurrentHP];
            SetStat(StatType.CurrentHP, cur - 10f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) // ���� �Ҹ�
        {
            float cur = stats[StatType.CurrentMP];
            SetStat(StatType.CurrentMP, Mathf.Max(0, cur - 10f));
        }

        TestExp();

        Debug.Log($"현재속도:{stats[StatType.SPEED]}");
    }
    private void InitializeStats()
    {
        stats[StatType.CurrentHP] = 100f; //���� HP
        stats[StatType.MaxHP] = 100f; //�ʱ� HP
        stats[StatType.CurrentMP] = 100f; //���� MP
        stats[StatType.MaxMP] = 100f; //�ʱ� MP
        stats[StatType.ATK] = 10f; //�ʱ� ���ݷ�
        stats[StatType.DEF] = 10f; //�ʱ� ����
        stats[StatType.LEVEL] = 1f; //�ʱ� ����
        stats[StatType.EXP] = 0f; //�ʱ� ����ġ
        stats[StatType.MaxEXP] = 0f; //�ʱ� ����ġ
        stats[StatType.GOLD] = 0f; //�ʱ� ���
        stats[StatType.SPEED] = 3f; //�ʱ� ���
        stats[StatType.CRITICAL] = 50f; //�ʱ� ũ��Ƽ�� Ȯ��
        stats[StatType.CRITICAL_DAMAGE] = 150f; //�ʱ� ũ��Ƽ�� ������ ����
        stats[StatType.COOLDOWN_REDUCTION] = 40f; //�ʱ� ��Ÿ�� ���� ����
    }

    // ������ ���� ������ �ʱ�ȭ
    private void InitializeLevelStats()
    {
        // ������ ���� ������ ���� (���� 2���� ����)
        for (int level = 2; level <= maxLevel; level++)
        {
            Dictionary<StatType, float> statIncreases = new Dictionary<StatType, float>();

            // ������ ������
            statIncreases[StatType.MaxHP] = 20f;         // HP ������
            statIncreases[StatType.MaxMP] = 15f;        // MP ������
            statIncreases[StatType.ATK] = 1f;     // ���ݷ� ������
            statIncreases[StatType.DEF] = 1f;     // ���� ������

            levelStats[level] = statIncreases;
        }
    }

    private void InitializeExpRequired()
    {
        // ������ �ʿ� ����ġ ����
        for (int level = 1; level <= maxLevel; level++)
        {
            // ����ġ ���� (��: level^2 * 100)
            expRequiredForLevel[level] = level * 100f;
        }

        int currentLevel = (int)stats[StatType.LEVEL];
        stats[StatType.MaxEXP] = expRequiredForLevel[currentLevel];
    }
    // ���� ����Ʈ �ý��� �ʱ�ȭ
    private void InitializeStatPointSystem()
    {
        // �� ���Ⱥ� ���ڵ� ����Ʈ �ʱ�ȭ
        foreach (StatType statType in investableStats)
        {
            investedStatPoints[statType] = 0;
        }

        // ���� ����Ʈ�� ������ ����
        statPointIncrease[StatType.ATK] = 1f;       // ���ݷ� ������
        statPointIncrease[StatType.CRITICAL] = 1f;  // ġ��Ÿ Ȯ�� ������
        statPointIncrease[StatType.MaxHP] = 10f;    // �ִ� ü�� ������
        statPointIncrease[StatType.DEF] = 1f;       // ���� ������
        statPointIncrease[StatType.SPEED] = 0.2f;   // �̵��ӵ� ������

        // �ִ� ���� ���� ����Ʈ ����
        maxStatInvestment[StatType.ATK] = 10;      // �ִ� ATK ���� ����Ʈ
        maxStatInvestment[StatType.CRITICAL] = 10;  // �ִ� CRITICAL ���� ����Ʈ (�ִ� 50% �߰�)
        maxStatInvestment[StatType.MaxHP] = 10;    // �ִ� MaxHP ���� ����Ʈ
        maxStatInvestment[StatType.DEF] = 10;      // �ִ� DEF ���� ����Ʈ
        maxStatInvestment[StatType.SPEED] = 10;     // �ִ� SPEED ���� ����Ʈ
    }

    

    // ���� ����Ʈ ���� �� ������ ��ȯ
    public float GetStatIncreasePerPoint(StatType statType)
    {
        if (statPointIncrease.ContainsKey(statType))
        {
            return statPointIncrease[statType];
        }
        return 0f;
    }

    // ����ġ ȹ�� �޼���
    public void GainExperience(float amount)
    {
        SetStat(StatType.EXP, stats[StatType.EXP] + amount);
        //stats[StatType.EXP] += amount;
        Debug.Log($"����ġ ȹ��: +{amount} (����: {stats[StatType.EXP]})");

        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        int curLevel = (int)stats[StatType.LEVEL];

        // �ִ� ������ �����ߴ��� üũ
        if (curLevel >= maxLevel)
        {
            stats[StatType.EXP] = expRequiredForLevel[maxLevel]; // ����ġ ����
            return;
        }

        // ���� �������� �ʿ��� ����ġ�� �Ѿ����� üũ
        if (stats[StatType.EXP] >= expRequiredForLevel[curLevel])
        {
            LevelUp();

            // ���� ����ġ�� �� �������� ������� Ȯ�� (���� ������ ó��)
            CheckLevelUp();
        }
    }

    private void LevelUp()
    {
        int newLevel = (int)stats[StatType.LEVEL] + 1;

        // ����ġ ���
        stats[StatType.EXP] -= expRequiredForLevel[(int)stats[StatType.LEVEL]];

        // ���� ����
        stats[StatType.LEVEL] = newLevel;

        // ������ ���� ���� ����
        ApplyLevelStats(newLevel);

        // ���� ����Ʈ �߰�
        AddStatPoints(statPointsPerLevel);

        Debug.Log($"스탯 포인트: {availableStatPoints}");
        if (passiveUI != null)
        {
            passiveUI.UpdateStatPointsUI(availableStatPoints);
        }

        stats[StatType.CurrentHP] = stats[StatType.MaxHP];
        stats[StatType.CurrentMP] = stats[StatType.MaxMP];

        int currentLevel = (int)stats[StatType.LEVEL];
        stats[StatType.MaxEXP] = expRequiredForLevel[currentLevel];

        Debug.Log($"���� ����: {stats[StatType.LEVEL]}");
        Debug.Log($"HP: {stats[StatType.MaxHP]}");
        Debug.Log($"MP: {stats[StatType.MaxMP]}");
        Debug.Log($"ATK: {stats[StatType.ATK]}");
        Debug.Log($"DEF: {stats[StatType.DEF]}");

    }

    // ������ ���� ���� ����
    private void ApplyLevelStats(int level)
    {
        if (levelStats.ContainsKey(level))
        {
            Dictionary<StatType, float> statIncreases = levelStats[level];

            foreach (var statType in statIncreases.Keys)
            {
                float newValue = stats[statType] + statIncreases[statType];
                SetStat(statType, newValue); // �̺�Ʈ �߻� ����
                //stats[statType] += statIncreases[statType];
                Debug.Log($"{statType} ����: +{statIncreases[statType]}");
            }
        }
    }

    // ���� ����Ʈ �߰�    
    public void AddStatPoints(int points)
    {
        availableStatPoints += points;
        OnStatPointsChanged?.Invoke(availableStatPoints);
    }

    // ���� ����Ʈ ����
    public bool InvestStatPoint(StatType statType)
    {
        // ���� ������ �������� Ȯ��
        bool isInvestable = false;
        foreach (StatType type in investableStats)
        {
            if (type == statType)
            {
                isInvestable = true;
                break;
            }
        }
               
        // ��� ������ ���� ����Ʈ�� �ִ��� Ȯ��
        if (availableStatPoints <= 0)
        {
            Debug.LogWarning("��� ������ ���� ����Ʈ�� �����ϴ�.");
            return false;
        }

        // �ִ� ���� ���� ����Ʈ�� �ʰ��ϴ��� Ȯ��
        if (investedStatPoints[statType] >= maxStatInvestment[statType])
        {
            Debug.LogWarning($"{statType}�� �� �̻� ���� ����Ʈ�� ������ �� �����ϴ�. (�ִ�: {maxStatInvestment[statType]})");
            return false;
        }

        // ���� ����Ʈ ���
        availableStatPoints--;
        investedStatPoints[statType]++;

        // ���� ���� ����
        float newValue = stats[statType] + statPointIncrease[statType];
        SetStat(statType, newValue);

        // ���� ü�µ� �Բ� ���� (MaxHP ����Ʈ ���ڽ�)
        if (statType == StatType.MaxHP)
        {
            float currentHP = stats[StatType.CurrentHP];
            float increase = statPointIncrease[StatType.MaxHP];
            SetStat(StatType.CurrentHP, currentHP + increase);
        }

        // ���� ����Ʈ ���� �̺�Ʈ �߻�
        OnStatPointsChanged?.Invoke(availableStatPoints);

        Debug.Log($"{statType}�� ���� ����Ʈ�� �����߽��ϴ�. ({statType}: +{statPointIncrease[statType]}, �� ����: {investedStatPoints[statType]}/{maxStatInvestment[statType]})");
        Debug.Log($"���� ���� ����Ʈ: {availableStatPoints}");

        return true;
    }

    // ���� ����Ʈ ���� (��� ���� ���)
    public void ResetStatPoints()
    {
        int totalPoints = 0;

        // �� ���� ���� ������ ���� �� ���� ����Ʈ ȸ��
        foreach (StatType statType in investableStats)
        {
            if (investedStatPoints.ContainsKey(statType))
            {
                // ���ڵ� ����Ʈ ȸ��
                int pointsInvested = investedStatPoints[statType];
                totalPoints += pointsInvested;

                // ���� ���� �� ��� (���� �� - ���ڷ� ���� ������)
                float originalValue = stats[statType] - (pointsInvested * statPointIncrease[statType]);
                SetStat(statType, originalValue);

                // ���ڵ� ����Ʈ �ʱ�ȭ
                investedStatPoints[statType] = 0;
            }
        }

        // MaxHP ���� �� CurrentHP�� ����
        if (investedStatPoints.ContainsKey(StatType.MaxHP))
        {
            float currentHPRatio = stats[StatType.CurrentHP] / stats[StatType.MaxHP];
            SetStat(StatType.CurrentHP, stats[StatType.MaxHP] * currentHPRatio);
        }

        // ��� ������ ���� ����Ʈ ����
        availableStatPoints += totalPoints;
        OnStatPointsChanged?.Invoke(availableStatPoints);

        Debug.Log($"��� ���� ����Ʈ�� �ʱ�ȭ�Ǿ����ϴ�. (��� ������ ����Ʈ: {availableStatPoints})");
    }

    // ���� ��� ������ ���� ����Ʈ�� ��ȯ
    public int GetAvailableStatPoints()
    {
        return availableStatPoints;
    }

    // Ư�� ���ȿ� ���ڵ� ����Ʈ�� ��ȯ
    public int GetInvestedStatPoints(StatType statType)
    {
        if (investedStatPoints.ContainsKey(statType))
        {
            return investedStatPoints[statType];
        }
        return 0;
    }

    // Ư�� ������ �ִ� ���� ���� ����Ʈ�� ��ȯ
    public int GetMaxStatInvestment(StatType statType)
    {
        if (maxStatInvestment.ContainsKey(statType))
        {
            return maxStatInvestment[statType];
        }
        return 0;
    }
    private void GetGold() //��� ȹ�� 
    {

    }

    private void TestExp() //�׽�Ʈ ����ġ ȹ��
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
