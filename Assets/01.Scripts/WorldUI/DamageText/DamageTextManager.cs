using System.Collections;
using UnityEngine;

public class DamageTextManager : SingletonLoadRemain<DamageTextManager>
{
    public ScreenFader screenFader;
    public DamageTextPool pool;
    
    protected override void Init()
    {
        base.Init();

        screenFader = GetComponentInChildren<ScreenFader>();
        pool = GetComponentInChildren<DamageTextPool>();
    }

    public void ShowDamage(Vector3 position, int damage, bool isCritical)
    {
        GameObject obj = pool.Get();
        obj.transform.position = position;

        obj.GetComponent<DamageText>().Setup(damage, isCritical);
    }

    public void ShowExperience(int amount)
    {
        GameObject obj = pool.Get();
        obj.transform.position = GameManager.Instance.player.transform.position + (Vector3.up * 0.5f);
        
        obj.GetComponent<DamageText>().ShowEXP(amount);
    }

    public void ShowMessage(string message)
    {
        GameObject obj = pool.Get();
        obj.transform.position = GameManager.Instance.player.transform.position + (Vector3.up * 1.5f);

        obj.GetComponent<DamageText>().ShowMessage(message);
    }

    public void ShowDeath()
    {
        GameObject obj = pool.Get();

        obj.transform.position = Vector3.zero;
        UIManager.Instance.HideIngameUI();
        obj.GetComponent<DamageText>().Setup("You Die", Color.red);
    }

    public void ShowComboTiming(float duration)
    {
        GameObject obj = pool.Get();
        obj.transform.position = GameManager.Instance.player.transform.position + (Vector3.up * 1.2f);

        DamageText damageText = obj.GetComponent<DamageText>();
        StartCoroutine(UpdateComboTimer(damageText, duration));
    }

    IEnumerator UpdateComboTimer(DamageText damageText, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float remainingTime = duration - elapsedTime;

            damageText.ShowMessage($"콤보 입력 가능! ({remainingTime:F1}초)", Color.green);

            yield return null;
        }

        damageText.ShowMessage("");
    }
}
