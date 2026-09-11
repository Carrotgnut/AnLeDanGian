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
        // Không cho tương tác lại trong thời gian cooldown
        if (Time.time < nextAllowedInteractionTime)
        {
            return;
        }

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

        // Bắt đầu thời gian chờ trước khi cho phép tương tác lại
        nextAllowedInteractionTime = Time.time + reinteractDelay;

        if (animator != null)
        {
            animator.SetBool("IsTalking", false);
        }
    }
}