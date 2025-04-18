using UnityEngine;

public class DamageTextManager : Singleton <DamageTextManager>
{
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(transform.root.gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Init();
    }

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
