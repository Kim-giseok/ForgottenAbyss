using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class StatUI : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI expText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI mpText;
    public TextMeshProUGUI atkText;
    public TextMeshProUGUI defText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI crtText;
    public TextMeshProUGUI cdwText;

    public GameObject statusUI;
    public EquippedItemUI equippedItemUI;

    PlayerStatus playerStatus => GameManager.Instance.pStatus;
    //PlayerStatus playerStatus = PlayerStatus.Instance;

    private void Awake()
    {
        //playerStatus = PlayerStatus.Instance;
        //playerStatus = FindObjectOfType<PlayerStatus>();
    }

    private void OnEnable()
    {
        // 패널이 활성화될 때마다 최신 정보로 UI를 업데이트
        if (playerStatus != null)
        {
            // 이벤트 등록
            playerStatus.OnStatChanged += OnStatChanged;
            // 즉시 UI 업데이트
            UpdateUI();
        }
    }

    private void OnDisable()
    {
        // 패널이 비활성화될 때 이벤트 구독 해제
        if (playerStatus != null)
        {
            playerStatus.OnStatChanged -= OnStatChanged;
        }
    }

    // 스탯 변경 이벤트 핸들러
    private void OnStatChanged(StatType type, float newValue)
    {
        // 활성화된 상태에서만 UI 업데이트
        if (gameObject.activeSelf)
        {
            UpdateUI();
        }
    }
    public void UpdateUI()
    {
        levelText.text = $"Lv " + playerStatus.stats[StatType.LEVEL].ToString();
        expText.text = $"Exp: " + FormatStat(playerStatus.stats[StatType.EXP]) + $"/" + FormatStat(playerStatus.stats[StatType.MaxEXP]);
        hpText.text = $"HP: " + FormatStat(playerStatus.stats[StatType.CurrentHP]) + $"/" + FormatStat(playerStatus.stats[StatType.MaxHP]);
        mpText.text = $"MP: " + FormatStat(playerStatus.stats[StatType.CurrentMP]) + $"/" + FormatStat(playerStatus.stats[StatType.MaxMP]);
        atkText.text = $"공격력: " + FormatStat(playerStatus.stats[StatType.ATK]);
        defText.text = $"방어력: " + FormatStat(playerStatus.stats[StatType.DEF]);
        speedText.text = $"이동속도: " + FormatStat(playerStatus.stats[StatType.SPEED]);
        crtText.text = $"치명타 확률: " + FormatStat(playerStatus.stats[StatType.CRITICAL]) + "%";
        cdwText.text = $"스킬 쿨타임 감소: " + FormatStat(playerStatus.stats[StatType.COOLDOWN_REDUCTION]) + "%";
    }

    private string FormatStat(float value)
    {
        return (value % 1 == 0) ? Mathf.FloorToInt(value).ToString() : value.ToString("F1");
    }

    public void OnStatusUI()
    {
        statusUI.SetActive(true);
        UpdateUI();
    }

    public void OffStatUI()
    {
        statusUI.SetActive(false);
    }
}
