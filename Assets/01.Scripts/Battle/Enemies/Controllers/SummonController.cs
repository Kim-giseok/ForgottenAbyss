using System;
using UnityEngine;

public class SummonController: EnemyBaseController
{
    // notice: 소환 기술을 위한 정보들
    [HideInInspector] public string SkillNodeName; // notice: 주입할 skill 자체를
    
    private void Start()
    {
        // 프리팹 자체에서 지정하기
        spriteRenderer.color = Color.black; // shader 적용하기
        gameObject.layer = LayerMask.NameToLayer("Player");
        
        // player를 contoller로 그냥 빼는 게 편할 듯 - player어가 사라지면 재 탐색
        // notice: 플레이어 이동이 필요한 경우
        // 플레이어 비/활성화가 잠시 필요
        // player.GetComponent<SpriteRenderer>().enabled = false;
        // player.GetComponent<Collider2D>().enabled = false;
        // player.GetComponent<Rigidbody2D>().gravityScale = 0;
        // player.transform.position = transform.position;
        // Destroy(gameObject);
        
        // var currNode = NodeManager.allNodes.Find(node => node.name == SkillNodeName).node; // 스킬 자체를 데이터셋으로 가진다면
    }
}