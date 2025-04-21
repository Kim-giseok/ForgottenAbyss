using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance { get; private set; }

    [SerializeField] private int currentGold = 1000; // 현재 소지 금액
    [SerializeField] private List<TextMeshProUGUI> goldTexts;

    private void Awake()
    {
        // 싱글톤 패턴
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        UpdateGoldUI();
    }

    public int GetGold() => currentGold;

    public bool SpendGold(int amount)
    {
        if (currentGold >= amount)
        {
            currentGold -= amount;
            UpdateGoldUI();
            return true;
        }
        return false;
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
        UpdateGoldUI();
    }

    private void UpdateGoldUI()
    {
        foreach (var goldtxt in goldTexts)
        {
            goldtxt.text = currentGold.ToString() + "";
        }
    }
}
