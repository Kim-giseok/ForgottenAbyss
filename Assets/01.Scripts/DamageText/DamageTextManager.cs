using UnityEngine;

public class DamageTextManager : Singleton <DamageTextManager>
{
    public void ShowDamage(Vector3 position, int damage)
    {
        GameObject obj = DamageTextPool.Instance.Get();
        obj.transform.position = position;

        obj.GetComponent<DamageText>().Setup(damage);
    }

    public void ShowDeath()
    {
        GameObject obj = DamageTextPool.Instance.Get();

        obj.transform.position = Vector3.zero;

        obj.GetComponent<DamageText>().Setup("You Die", Color.red);
    }
}
