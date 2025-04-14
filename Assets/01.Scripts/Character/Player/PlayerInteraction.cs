using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange; //상호작용 범위
    public LayerMask interactableLayer; //상호작용 가능한 오브젝트의 레이어
    public GameObject npcText; //npc에게 다가가면 활성화 되는 안내문구

    public void Update()
    {
        Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y + GetComponent<Collider2D>().bounds.extents.y); ; // Raycast 시작점
        Vector2 rayDirection = transform.right; // 오른쪽 방향으로 Raycast

        Debug.DrawRay(rayOrigin, rayDirection * interactionRange, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, 1f, interactableLayer);

        if (hit.collider != null && hit.collider.CompareTag("Npc"))
        {
            if (FindObjectOfType<TalkSystem>() != null)
            {
                npcText.SetActive(false); //대화창이 켜져 있으면 안내문구 비활성화
                return;  
            }
            else
            {
                npcText.SetActive(true);
            }

        }
        else
        {
            npcText.SetActive(false);
        }

    }

    public void Interact(Vector2 origin, Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, interactionRange, interactableLayer); // 2D Raycast
       
        if (hit.collider != null)
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                interactable.ActiveInteraction();
            }
        }
    }

}  
