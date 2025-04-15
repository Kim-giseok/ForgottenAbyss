using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIBinder : MonoBehaviour
{
    public PlayerStatus playerStatus; // PlayerStatus 연결
    public Slider hpSlider; // 게이지바
    public float maxHP = 100f; // 최대 HP

    private void Start()
    {
        // 초기 최대 HP PlayerStatus에서 가져오기
        if (playerStatus != null && playerStatus.stats.ContainsKey(StatType.HP))
        {
            maxHP = playerStatus.stats[StatType.HP];
        }

        // 슬라이더 최대값 설정
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
