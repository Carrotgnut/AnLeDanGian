using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private string startDialogueId = "ch1_001";

    public void Interact()
    {
        if (dialogueManager == null)
        {
            Debug.LogError(
                "NPCDialogue: Chưa gán DialogueManager!"
            );
            return;
        }

        if (dialogueManager.IsDialogueActive())
            return;

        dialogueManager.StartDialogue(startDialogueId);
    }
}