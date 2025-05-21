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
}
