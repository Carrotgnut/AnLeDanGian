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

    private IInteractable currentInteractable;

    private void Update()
    {
        CheckForInteractable();
    }

    private void CheckForInteractable()
    {
        currentInteractable = null;

        if (playerCamera == null || interactionUI == null)
        {
            return;
        }

        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
        {
            currentInteractable = hit.collider.GetComponent<IInteractable>();

            if (currentInteractable != null)
            {
                interactionUI.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    currentInteractable.Interact();
                }

                return;
            }
        }

        interactionUI.SetActive(false);
    }
}