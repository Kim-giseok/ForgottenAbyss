using UnityEngine;

public class DamageTextManager : SingletonLoadRemain<DamageTextManager>
{
    public void ShowDamage(Vector3 position, int damage, bool isCritical)
    {
        GameObject obj = DamageTextPool.Instance.Get();
        obj.transform.position = position;

        obj.GetComponent<DamageText>().Setup(damage, isCritical);
    }

    public void ShowDeath()
    {
        GameObject obj = DamageTextPool.Instance.Get();

        obj.transform.position = Vector3.zero;
        UIManager.Instance.HideIngameUI();
        obj.GetComponent<DamageText>().Setup("You Die", Color.red);
    }
}
