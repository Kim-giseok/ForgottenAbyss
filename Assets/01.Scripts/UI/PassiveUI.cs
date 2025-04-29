using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PassiveUI : MonoBehaviour
{
    public GameObject passiveUI;

    public TextMeshProUGUI atkText;
    public TextMeshProUGUI crtText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI defText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI coolDownText;
    public TextMeshProUGUI statPointText;

    // 각 스탯 버튼에 대한 참조 추가
    public Button atkButton;
    public Button crtButton;
    public Button hpButton;
    public Button defButton;
    public Button speedButton;
    public Button coolDownButton;

    private int atkLevel = 0;
    private int maxAtkLevel = 10;
    private int crtLevel = 0;
    private int maxCrtLevel = 10;
    private int hpLevel = 0;
    private int maxHpLevel = 10;
    private int defLevel = 0;
    private int maxDefLevel = 10;
    private int speedLevel = 0;
    private int maxSpeedLevel = 10;
    private int coolDownLevel = 0;
    private int maxCoolDownLevel = 10;

    //PlayerStatus _playerStatus = PlayerStatus.Instance;
    PlayerStatus _playerStatus => GameManager.Instance.pStatus;

    //private void Awake()
    //{
    //    _playerStatus = FindObjectOfType<PlayerStatus>();
    //    //_playerStatus = PlayerStatus.Instance;

    //}

    //private void Start()
    //{
    //    // PlayerStatus에서 스탯 포인트 변경 이벤트 구독
    //    if (_playerStatus != null)
    //    {
    //        _playerStatus.OnStatPointsChanged += UpdateStatPointsUI;
    //    }

    //    // 초기 UI 상태 업데이트
    //    UpdateStatPointsUI(_playerStatus.GetAvailableStatPoints());
    //}

    private void OnDestroy()
    {
        // 이벤트 구독 해제
        if (_playerStatus != null)
        {
            _playerStatus.OnStatPointsChanged -= UpdateStatPointsUI;
        }
    }

    public void UpdateStatPointsUI(int availablePoints)
    {
        // 스탯 포인트 텍스트 업데이트
        if (statPointText != null)
        {
            statPointText.text = $"StatPoint: {availablePoints}";
        }

        // 스탯 포인트가 0이면 모든 버튼 비활성화
        bool hasPoints = availablePoints > 0;

        // 각 버튼의 활성화 상태 결정 (스탯 포인트가 있고 최대 레벨이 아닐 때만 활성화)
        if (atkButton != null) atkButton.interactable = hasPoints && atkLevel < maxAtkLevel;
        if (crtButton != null) crtButton.interactable = hasPoints && crtLevel < maxCrtLevel;
        if (hpButton != null) hpButton.interactable = hasPoints && hpLevel < maxHpLevel;
        if (defButton != null) defButton.interactable = hasPoints && defLevel < maxDefLevel;
        if (speedButton != null) speedButton.interactable = hasPoints && speedLevel < maxSpeedLevel;
        if (coolDownButton != null) coolDownButton.interactable = hasPoints && coolDownLevel < maxCoolDownLevel;
    }
    public void OnPassiveUI()
    {
        passiveUI.SetActive(true);
    }

    public void OffPassiveUI()
    {
        passiveUI.SetActive(false);
    }

    public void OnClickAttack()
    {
        if (_playerStatus.GetAvailableStatPoints() <= 0 || atkLevel >= maxAtkLevel)
        {
            return;
        }

        float currenAtk = _playerStatus.GetStat(StatType.ATK);
        _playerStatus.SetStat(StatType.ATK, currenAtk + 1);
        Debug.Log($"공격력 증가: {_playerStatus.GetStat(StatType.ATK)}");
        _playerStatus.AddStatPoints(-1);

        if (atkLevel >= maxAtkLevel)
        {
            return;
        }

        atkLevel++;
        atkText.text = $"ATK\n Lv {atkLevel}/{maxAtkLevel}";
    }

    public void OnClickCritical()
    {
        if (_playerStatus.GetAvailableStatPoints() <= 0 || atkLevel >= maxAtkLevel)
        {
            return;
        }

        float currentCrt = _playerStatus.GetStat(StatType.CRITICAL);
        _playerStatus.SetStat(StatType.CRITICAL, currentCrt + 3);
        _playerStatus.AddStatPoints(-1);

        if (crtLevel >= maxCrtLevel)
        {
            return;
        }

        crtLevel++;
        crtText.text = $"CRT\n Lv {crtLevel}/{maxCrtLevel}";
    }

    public void OnClickHp()
    {
        if (_playerStatus.GetAvailableStatPoints() <= 0 || atkLevel >= maxAtkLevel)
        {
            return;
        }

        float currentHp = _playerStatus.GetStat(StatType.MaxHP);
        _playerStatus.SetStat(StatType.MaxHP, currentHp + 10);
        _playerStatus.AddStatPoints(-1);

        if (hpLevel >= maxHpLevel)
        {
            return;
        }

        hpLevel++;
        hpText.text = $"HP\n Lv {hpLevel}/{maxHpLevel}";
    }

    public void OnClickDefence()
    {
        if (_playerStatus.GetAvailableStatPoints() <= 0 || atkLevel >= maxAtkLevel)
        {
            return;
        }

        float currentDef = _playerStatus.GetStat(StatType.DEF);
        _playerStatus.SetStat(StatType.DEF, currentDef + 1);
        _playerStatus.AddStatPoints(-1);

        if (defLevel >= maxDefLevel)
        {
            return;
        }

        defLevel++;
        defText.text = $"DEF\n Lv {defLevel}/{maxDefLevel}";    
    }

    public void OnClickSpeed()
    {
        if (_playerStatus.GetAvailableStatPoints() <= 0 || atkLevel >= maxAtkLevel)
        {
            return;
        }

        float currentSpd = _playerStatus.GetStat(StatType.SPEED);
        _playerStatus.SetStat(StatType.SPEED, currentSpd + 0.2f);
        _playerStatus.AddStatPoints(-1);

        if (speedLevel >= maxSpeedLevel)
        {
            return;
        }

        speedLevel++;
        speedText.text = $"SPEED\n Lv {speedLevel}/{maxSpeedLevel}";
    }

    public void OnClickCoolDown()
    {
        if (_playerStatus.GetAvailableStatPoints() <= 0 || atkLevel >= maxAtkLevel)
        {
            return;
        }

        float currentCdw = _playerStatus.GetStat(StatType.COOLDOWN_REDUCTION);
        _playerStatus.SetStat(StatType.COOLDOWN_REDUCTION, currentCdw + 5f);
        _playerStatus.AddStatPoints(-1);

        if (speedLevel >= maxSpeedLevel)
        {
            return;
        }

        coolDownLevel++;
        coolDownText.text = $"COOLDOWN\n Lv {coolDownLevel}/{maxCoolDownLevel}";
    }

}
