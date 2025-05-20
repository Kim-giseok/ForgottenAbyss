using UnityEngine;
using UnityEngine.Serialization;

public class EventMoonstoneDeath: MonoBehaviour, IDamagable
{
    private EnemyController controller;
    public GameObject endingCutScene;
    
    public void GetDamage(float damage)
    {
        if (controller.resourceHandler.Get(EnemyStatType.Health).currValue - damage < 0)
        {
            endingCutScene.SetActive(true);
            controller.Machine.SetPlaying(false);
            return;
        }
        
        controller.GetDamage(damage);

    }
}