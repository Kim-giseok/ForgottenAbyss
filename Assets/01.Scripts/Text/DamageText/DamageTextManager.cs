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

    public void ShowDeath()
    {
        GameObject obj = pool.Get();

        obj.transform.position = Vector3.zero;
        UIManager.Instance.HideIngameUI();
        obj.GetComponent<DamageText>().Setup("You Die", Color.red);
    }
}
