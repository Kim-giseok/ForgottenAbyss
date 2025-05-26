using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldTextUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText; // 골드텍스트 연동

    private void OnEnable()
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

    private IEnumerator Start()
    {
        yield return null; // 한 프레임 기다림
        if (GoldManager.Instance != null)
            UpdateGoldText(GoldManager.Instance.GetGold());
    }

    private void UpdateGoldText(int amount)
    {
        goldText.text = $"{amount:N0}";
    }
}
