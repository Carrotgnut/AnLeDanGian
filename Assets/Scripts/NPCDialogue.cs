using UnityEngine;

public class NPCDialogue : MonoBehaviour, IInteractable
{
    [Header("Dialogue")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private string startDialogueId = "ch1_001";

    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("Interaction Cooldown")]
    [SerializeField] private float reinteractDelay = 1f;

    private bool isTalking;
    private float nextAllowedInteractionTime;

    public void Interact()
    {
        if (Time.time < nextAllowedInteractionTime)
        {
            return;
        }

        if (dialogueManager == null)
        {
            Debug.LogError(
                "NPCDialogue: Chưa gán DialogueManager."
            );

            return;
        }

        if (dialogueManager.IsDialogueActive())
        {
            return;
        }

        if (dialogueManager.LastDialogueEndedFrame ==
            Time.frameCount)
        {
            return;
        }

        if (isTalking)
        {
            return;
        }

        isTalking = true;

        if (animator != null)
        {
            animator.SetBool("IsTalking", true);
        }

        // Cô Hiền chỉ dùng DialogueManager của chapter1.json.
        dialogueManager.StartDialogue(startDialogueId);
    }

    public void StopTalking()
    {
        isTalking = false;
        nextAllowedInteractionTime =
            Time.time + reinteractDelay;

        if (animator != null)
        {
            animator.SetBool("IsTalking", false);
        }
    }
}