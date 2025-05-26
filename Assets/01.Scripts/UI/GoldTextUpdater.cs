using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldTextUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText; // 골드텍스트 연동
    private Coroutine updateCoroutine;

    private void OnEnable()
    {
        updateCoroutine = StartCoroutine(DeferredSubscribe());
    }

    private void OnDisable()
    {
        if (updateCoroutine != null)
        {
            StopCoroutine(updateCoroutine);
            updateCoroutine = null;
        }

        if (GoldManager.Instance != null)
            GoldManager.Instance.OnGoldChanged -= UpdateGoldText;
    }

    private IEnumerator DeferredSubscribe()
    {
        yield return null;

        if (GoldManager.Instance != null)
        {
            GoldManager.Instance.OnGoldChanged -= UpdateGoldText;
            GoldManager.Instance.OnGoldChanged += UpdateGoldText;

            UpdateGoldText(GoldManager.Instance.GetGold());
        }
    }

    private void UpdateGoldText(int amount)
    {
        goldText.text = $"{amount:N0}";
    }
}
