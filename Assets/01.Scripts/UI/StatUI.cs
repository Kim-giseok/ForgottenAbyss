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
        expText.text = $"Exp: " + playerStatus.stats[StatType.EXP].ToString() + $"/" + playerStatus.stats[StatType.MaxEXP];
        hpText.text = $"HP: " + playerStatus.stats[StatType.CurrentHP].ToString() + $"/" + playerStatus.stats[StatType.MaxHP];
        mpText.text = $"MP: " + playerStatus.stats[StatType.CurrentMP].ToString() + $"/" + playerStatus.stats[StatType.MaxMP];
        atkText.text = $"ATK: " + playerStatus.stats[StatType.ATK].ToString();
        defText.text = $"DEF: " + playerStatus.stats[StatType.DEF].ToString();
        speedText.text = $"SPEED: " + playerStatus.stats[StatType.SPEED].ToString();
        crtText.text = $"CRITICAL: " + playerStatus.stats[StatType.CRITICAL].ToString();
        cdwText.text = $"COOLDOWN: " + playerStatus.stats[StatType.COOLDOWN_REDUCTION].ToString() + "%";
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
