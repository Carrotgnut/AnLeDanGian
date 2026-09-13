using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string clueId;

    public void Interact()
    {
        if (ClueManager.Instance == null)
        {
            Debug.LogError("ClueInteractable: Chưa có ClueManager trong Scene.");
            return;
        }

        ClueManager.Instance.TryDiscover(clueId);
    }

    public string GetInteractionPrompt()
    {
        return "[E] Khám phá";
    }
}