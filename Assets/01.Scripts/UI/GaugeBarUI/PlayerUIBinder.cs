using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIBinder : MonoBehaviour
{
    public PlayerStatus playerStatus; // PlayerStatus 연결
    public Slider hpSlider; // HP 게이지바
    public TMPro.TextMeshProUGUI hpText; // HP텍스트
    public Slider mpSlider; // MP 게이지바
    public TMPro.TextMeshProUGUI mpText; // MP텍스트

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
        mpSlider.minValue = 0f;
        mpSlider.maxValue = 1f;

        //초기값 반영
        UpdateHPSlider();
        UpdateMPSlider();
    }

    private void HandleStatChanged(StatType type, float newValue)
    {
        if (type == StatType.CurrentHP || type == StatType.MaxHP)
        {
            //Debug.Log($"[UIBinder] 체력 반영됨: {newValue}");
            UpdateHPSlider();
        }

        if (type == StatType.CurrentMP || type == StatType.MaxMP)
        {
            //Debug.Log($"[UIBinder] 마나 반영됨: {newValue}");
            UpdateMPSlider();
        }
    }

    // HP UI갱신 함수
    private void UpdateHPSlider()
    {
        if (playerStatus == null) return;
        if (!playerStatus.stats.ContainsKey(StatType.CurrentHP) || !playerStatus.stats.ContainsKey(StatType.MaxHP))
            return;

        float curHP = playerStatus.stats[StatType.CurrentHP];
        float maxHP = playerStatus.stats[StatType.MaxHP];

        hpSlider.value = Mathf.Clamp01(curHP / maxHP);

        // text UI
        if (hpText != null)
        {
            hpText.text = $"{(int)curHP} / {(int)maxHP}";
        }
    }

    // MP UI갱신 함수
    private void UpdateMPSlider()
    {
        if (playerStatus == null) return;
        if (!playerStatus.stats.ContainsKey(StatType.CurrentMP) || !playerStatus.stats.ContainsKey(StatType.MaxMP))
            return;

        float curMP = playerStatus.stats[StatType.CurrentMP];
        float maxMP = playerStatus.stats[StatType.MaxMP];

        mpSlider.value = Mathf.Clamp01(curMP / maxMP);

        if (mpText != null)
        {
            mpText.text = $"{(int)curMP} / {(int)maxMP}";
        }
    }
}
