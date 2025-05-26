using UnityEngine;
using UnityEngine.Serialization;

public class EventMoonstoneDeath: MonoBehaviour, IDamagable
{
    private EnemyController controller;
    public GameObject endingCutScene;

    private void Awake()
    {
        controller = GetComponent<EnemyController>();
    }
    
    public void GetDamage(float damage)
    {
        if (controller.Resource.Get(EnemyStatType.Health).value - damage < 0)
        {
            controller.Anim.Play("Hit");
            controller.Machine.SetPlaying(false);
            endingCutScene.SetActive(true);
            return;
        }
        
        controller.GetDamage(damage);

    }
}