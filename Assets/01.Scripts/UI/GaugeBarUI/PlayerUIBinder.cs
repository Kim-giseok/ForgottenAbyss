using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIBinder : MonoBehaviour
{
    public PlayerStatus playerStatus; // PlayerStatus 연결
    public Slider hpSlider; // HP 게이지바

    private void OnEnable()
    {
        if (playerStatus != null)
            playerStatus.OnStatChanged += HandleStatChanged;
    }

    private void OnDisable()
    {
        if (playerStatus != null)
            playerStatus.OnStatChanged -= HandleStatChanged;
    }

    public void BindStatus(PlayerStatus status)
    {
        playerStatus = status;

        // 이벤트 구독
        playerStatus.OnStatChanged += HandleStatChanged;

        // 슬라이더 초기 설정
        hpSlider.minValue = 0f;
        hpSlider.maxValue = 1f;

        //초기값 반영
        UpdateSlider();
    }

    private void HandleStatChanged(StatType type, float newValue)
    {
        if (type == StatType.CurrentHP || type == StatType.MaxHP)
        {
            Debug.Log($"[UIBinder] 체력 반영됨: {newValue}");
            UpdateSlider();
        }
    }

    private void UpdateSlider()
    {
        if (playerStatus == null) return;
        if (!playerStatus.stats.ContainsKey(StatType.CurrentHP) || !playerStatus.stats.ContainsKey(StatType.MaxHP))
            return;

        float cur = playerStatus.stats[StatType.CurrentHP];
        float max = playerStatus.stats[StatType.MaxHP];

        hpSlider.value = Mathf.Clamp01(cur / max);
    }
}
