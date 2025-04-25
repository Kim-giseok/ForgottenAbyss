using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PassiveUI : MonoBehaviour
{
    public GameObject passiveUI;

    public TextMeshProUGUI atkText;
    public TextMeshProUGUI crtText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI defText;
    public TextMeshProUGUI speedText;

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

    //PlayerStatus _playerStatus = PlayerStatus.Instance;
    PlayerStatus _playerStatus;

    private void Awake()
    {
        _playerStatus = FindObjectOfType<PlayerStatus>();
        //_playerStatus = PlayerStatus.Instance;

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
        float currenAtk = _playerStatus.GetStat(StatType.ATK);
        _playerStatus.SetStat(StatType.ATK, currenAtk + 1);
        Debug.Log($"공격력 증가: {_playerStatus.GetStat(StatType.ATK)}");
        
        if (atkLevel >= maxAtkLevel)
        {
            return;
        }

        atkLevel++;
        atkText.text = $"ATK\n Lv {atkLevel}/{maxAtkLevel}";
    }

    public void OnClickCritical()
    {
        float currentCrt = _playerStatus.GetStat(StatType.CRITICAL);
        _playerStatus.SetStat(StatType.CRITICAL, currentCrt + 3);

        if (crtLevel >= maxCrtLevel)
        {
            return;
        }

        crtLevel++;
        crtText.text = $"CRT\n Lv {crtLevel}/{maxCrtLevel}";
    }

    public void OnClickHp()
    {
        float currentHp = _playerStatus.GetStat(StatType.MaxHP);
        _playerStatus.SetStat(StatType.MaxHP, currentHp + 10);

        if (hpLevel >= maxHpLevel)
        {
            return;
        }

        hpLevel++;
        hpText.text = $"HP\n Lv {hpLevel}/{maxHpLevel}";
    }

    public void OnClickDefence()
    {
        float currentDef = _playerStatus.GetStat(StatType.DEF);
        _playerStatus.SetStat(StatType.DEF, currentDef + 1);

        if (defLevel >= maxDefLevel)
        {
            return;
        }

        defLevel++;
        defText.text = $"DEF\n Lv {defLevel}/{maxDefLevel}";
    }

    public void OnClickSpeed()
    {
        float currentSpd = _playerStatus.GetStat(StatType.SPEED);
        _playerStatus.SetStat(StatType.SPEED, currentSpd + 0.1f);

        if (speedLevel >= maxSpeedLevel)
        {
            return;
        }

        speedLevel++;
        speedText.text = $"SPEED\n Lv {speedLevel}/{maxSpeedLevel}";
    }

    public void OnClickCoolDown()
    {
        
    }

}
