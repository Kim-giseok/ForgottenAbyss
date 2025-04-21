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
    

    PlayerStatus playerStatus;
    

    private void Awake()
    {
        playerStatus = GetComponent<PlayerStatus>();
    }

    private void Update()
    {
        UpdateUI();
    }
    public void UpdateUI()
    {
        levelText.text = $"Lv " + playerStatus.stats[StatType.LEVEL].ToString();
        expText.text = $"Exp: " + playerStatus.stats[StatType.EXP].ToString() + $"/" + playerStatus.stats[StatType.MaxEXP];
        hpText.text = $"HP: " + playerStatus.stats[StatType.CurrentHP].ToString() + $"/" + playerStatus.stats[StatType.MaxHP];
        mpText.text = $"MP: " + playerStatus.stats[StatType.CurrentMP].ToString() + $"/" + playerStatus.stats[StatType.MaxMP];
        atkText.text = $"ATK: " + playerStatus.stats[StatType.ATK].ToString();
        defText.text = $"DEF: " + playerStatus.stats[StatType.DEF].ToString();
    }
}
