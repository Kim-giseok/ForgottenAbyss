using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange; //상호작용 범위
    public LayerMask interactableLayer; //상호작용 가능한 오브젝트의 레이어

    private void Update()
    {
        Vector2 rayOrigin = transform.position; // Raycast 시작점
        Vector2 rayDirection = transform.right; // 오른쪽 방향으로 Raycast


        Debug.DrawRay(rayOrigin, rayDirection * interactionRange, Color.red);

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
