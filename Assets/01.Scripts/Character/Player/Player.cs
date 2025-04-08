using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamagable
{
    // 일반 스킬 슬롯
    // 특수 스킬 슬롯
    // 레벨 업 시스템
       
    PlayerStatus playerstatus;

    private bool isDead = false;

    public void Awake()
    {
        playerstatus = GetComponent<PlayerStatus>();
    }

    
    public void GetDamage(float damage)
    {
        playerstatus.stats[StatType.HP] -= damage; //피격 시 데미지

        if (playerstatus.stats[StatType.HP] <= 0) //HP=0 되면 사망
        {
            isDead = true;
        }
    }


}
