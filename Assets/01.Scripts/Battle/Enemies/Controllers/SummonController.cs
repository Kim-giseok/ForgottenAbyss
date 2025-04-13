using System;
using UnityEngine;
using UnityEngine.Serialization;

public class SummonController: EnemyBaseController
{
    public void Set(string name, string skillName)
    {
        // 플레이어 비/활성화가 잠시 필요 - agent 쪽에서 인식 처리만 잘되면 됨
        // player.GetComponent<SpriteRenderer>().enabled = false;
        // player.GetComponent<Collider2D>().enabled = false;
        // player.GetComponent<Rigidbody2D>().gravityScale = 0;
        // player.transform.position = transform.position;
        
        animationHandler.SetController(EnemyAnimators.animators[name]);


        machine.OnLooped += () => { Destroy(gameObject); };
        machine.Define(Enemies.Get(name).skills[skillName]);
    }
}