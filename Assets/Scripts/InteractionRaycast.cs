using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionRaycast : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private Camera playerCamera;

    [Header("UI")]
    [SerializeField] private GameObject interactionUI;

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

        if (interactionUI == null)
        {
            Debug.LogWarning("InteractionRaycast: Chưa gán Interaction UI.");
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
                interactionUI.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    Interact(hit.collider.gameObject);
                }

                return;
            }
        }

        interactionUI.SetActive(false);
    }

    private void Interact(GameObject target)
    {
        Debug.Log("Đã tương tác với: " + target.name);   
    }
}