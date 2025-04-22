using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldTextUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText; // 골드텍스트 연동

    private void Start()
    {
        if (GoldManager.Instance != null)
        {
            GoldManager.Instance.OnGoldChanged += UpdateGoldText;
            UpdateGoldText(GoldManager.Instance.GetGold());
        }
    }

    private void OnDisable()
    {
        if (GoldManager.Instance != null)
            GoldManager.Instance.OnGoldChanged -= UpdateGoldText;
    }

    private void UpdateGoldText(int amount)
    {
        goldText.text = $"{amount:N0}";
    }
}
