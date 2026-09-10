using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour, IInteractable
{
    [Header("Dialogue")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private string startDialogueId = "ch1_001";

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private bool isTalking;

    public void Interact()
    {
        if (dialogueManager == null)
        {
            Debug.LogError("NPCDialogue: Chưa gán DialogueManager!");
            return;
        }

        if (isTalking || dialogueManager.IsDialogueActive())
        {
            return;
        }

        isTalking = true;

        if (animator != null)
        {
            animator.SetBool("IsTalking", true);
        }

        dialogueManager.StartDialogue(startDialogueId);
    }

    public void StopTalking()
    {
        isTalking = false;

        if (animator != null)
        {
            animator.SetBool("IsTalking", false);
        }
    }
}