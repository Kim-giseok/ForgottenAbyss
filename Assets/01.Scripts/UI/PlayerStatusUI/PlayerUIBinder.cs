using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIBinder : MonoBehaviour
{
    public PlayerStatus playerStatus; // PlayerStatus 연결
    public Slider hpSlider; // 게이지바
    public float maxHP = 100f; // 최대 HP

    public void BindStatus(PlayerStatus status)
    {
        playerStatus = status;

        if (playerStatus.stats.ContainsKey(StatType.HP))
            maxHP = playerStatus.stats[StatType.HP];

        hpSlider.minValue = 0f;
        hpSlider.maxValue = 1f;
    }

    private void Update()
    {
        if (playerStatus != null && playerStatus.stats.ContainsKey(StatType.HP))
        {
            float currentHP = playerStatus.stats[StatType.HP];
            float normalizedHP = currentHP / maxHP;

            hpSlider.value = Mathf.Clamp01(normalizedHP);
        }
    }
}
