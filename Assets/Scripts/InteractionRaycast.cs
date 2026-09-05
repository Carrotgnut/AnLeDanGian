using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionRaycast : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private Camera playerCamera;

    private void Update()
    {
        CheckForInteractable();
    }

    private void CheckForInteractable()
    {
        if (playerCamera == null)
        {
            Debug.LogWarning("InteractionRaycast: Chưa gán Player Camera.");
            return;
        }

        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                Debug.Log("Có thể tương tác với: " + hit.collider.gameObject.name);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    Interact(hit.collider.gameObject);
                }
            }
        }
    }

    private void Interact(GameObject target)
    {
        Debug.Log("Đã tương tác với: " + target.name);
    }
}