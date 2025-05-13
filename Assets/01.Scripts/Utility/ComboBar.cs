using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboBar : MonoBehaviour
{
    [SerializeField] private SpriteRenderer barSprite; // 게이지 스프라이트

    private float currentTime = 0f;
    private float maxTime = 1f;
    private bool isComboActive = false;
    private Vector3 initialScale;

    private void Awake()
    {
        initialScale = barSprite.transform.localScale;
    }

    private void Update()
    {
        if (isComboActive)
        {
            currentTime -= Time.deltaTime;
            float fillAmount = Mathf.Clamp01(currentTime / maxTime);
            barSprite.transform.localScale = new Vector3(initialScale.x * fillAmount, initialScale.y, initialScale.z);

            if (currentTime <= 0)
            {
                EndCombo();
            }
        }
    }

    public void StartCombo(float bufferTime)
    {
        isComboActive = true;
        maxTime = bufferTime;
        currentTime = maxTime;
        barSprite.gameObject.SetActive(true);
    }

    public void ContinueCombo(float nextBufferTime)
    {
        StartCombo(nextBufferTime);
    }

    public void EndCombo()
    {
        isComboActive = false;
        barSprite.gameObject.SetActive(false);
    }
}
