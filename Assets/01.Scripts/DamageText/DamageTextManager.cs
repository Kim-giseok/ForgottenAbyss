using UnityEngine;

public class DamageTextManager : Singleton <DamageTextManager>
{
    public void ShowDamage(Vector3 position, int damage)
    {
        GameObject obj = DamageTextPool.Instance.Get();
        obj.transform.position = position;

        obj.GetComponent<DamageText>().Setup(damage);
    }
}
