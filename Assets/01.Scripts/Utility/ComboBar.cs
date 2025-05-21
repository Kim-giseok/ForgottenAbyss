using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboBar : MonoBehaviour
{
    [SerializeField] private SpriteRenderer barSprite; // 게이지 스프라이트
    [SerializeField] private Color startColor = Color.green;
    [SerializeField] private Color endColor = Color.red;

    private float currentTime = 0f;
    private float maxTime = 1f;
    private bool isComboActive = false;
    private Vector3 initialScale;
    private int comboIndex = 0;

    public Animator comboEffectAnimator;

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

            barSprite.color = Color.Lerp(endColor, startColor, fillAmount);

            float newScaleX = Mathf.Abs(initialScale.x * fillAmount);
            barSprite.transform.localScale = new Vector3(newScaleX, initialScale.y, initialScale.z);

            if (currentTime <= 0)
            {
                EndCombo();
            }
        }
    }

    public void StartCombo(float bufferTime)
    {
        isComboActive = true;
        comboIndex++;
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
        comboIndex = 0;
        barSprite.gameObject.SetActive(false);
    }

    public void PlayEffect()
    {
        if (comboIndex != 0)
        {
            comboEffectAnimator.SetTrigger("ComboTrigger");
            StartCoroutine(DelayedEndCombo(0.1f));
        }
    }

    private IEnumerator DelayedEndCombo(float delay)
    {
        yield return new WaitForSeconds(delay);
        EndCombo();
    }
}
