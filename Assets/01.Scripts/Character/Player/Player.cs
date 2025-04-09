using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamagable
{
    // �Ϲ� ��ų ����
    // Ư�� ��ų ����
    // ���� �� �ý���
       
    PlayerStatus playerstatus;

    private bool isDead = false;

    public void Awake()
    {
        playerstatus = GetComponent<PlayerStatus>();
    }

    
    public void GetDamage(float damage)
    {
        playerstatus.stats[StatType.HP] -= damage; //�ǰ� �� ������

        if (playerstatus.stats[StatType.HP] <= 0) //HP=0 �Ǹ� ���
        {
            isDead = true;
        }
    }


}
