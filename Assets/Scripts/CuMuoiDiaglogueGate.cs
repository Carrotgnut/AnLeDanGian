using UnityEngine;

public class CuMuoiDialogueGate : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] private CuMuoiDialogueManager dialogueManager;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string talkingParameter = "IsTalking";

    [Header("Chapter 3")]
    [SerializeField] private TextAsset chapter3Json;
    [SerializeField] private string chapter3StartId = "ch3_001";

    [Header("Chưa đủ manh mối")]
    [SerializeField] private TextAsset lockedJson;
    [SerializeField] private string lockedStartId = "ch3_locked_001";

    [Header("Chapter 4")]
    [SerializeField] private TextAsset chapter4Json;
    [SerializeField] private string chapter4StartId = "ch4_001";

    [Header("State")]
    [SerializeField] private bool chapter3ConversationStarted;

    private bool isTalking;

    public void Interact()
    {
        if (dialogueManager == null)
        {
            Debug.LogError(
                "CuMuoiDialogueGate: Chưa gán CuMuoiDialogueManager."
            );

            return;
        }

        if (isTalking || dialogueManager.IsDialogueActive())
        {
            return;
        }

        if (!chapter3ConversationStarted)
        {
            bool started = StartDialogue(
                chapter3Json,
                chapter3StartId
            );

            if (started)
            {
                chapter3ConversationStarted = true;
            }

            return;
        }

        bool enoughClues =
            ClueManager.Instance != null &&
            ClueManager.Instance.HasEnoughCluesForTruth();

        if (!enoughClues)
        {
            StartDialogue(
                lockedJson != null
                    ? lockedJson
                    : chapter3Json,
                lockedStartId
            );

            return;
        }

        StartDialogue(
            chapter4Json,
            chapter4StartId
        );
    }

    private bool StartDialogue(
        TextAsset dialogueJson,
        string startId)
    {
        if (dialogueJson == null)
        {
            Debug.LogError(
                "CuMuoiDialogueGate: Chưa gán file JSON cho ID "
                + startId
            );

            return false;
        }

        bool started = dialogueManager.StartDialogue(
            dialogueJson,
            startId
        );

        if (started)
        {
            StartTalking();
        }

        return started;
    }

    private void StartTalking()
    {
        isTalking = true;

        if (animator != null)
        {
            animator.SetBool(talkingParameter, true);
        }
    }

    public void StopTalking()
    {
        isTalking = false;

        if (animator != null)
        {
            animator.SetBool(talkingParameter, false);
        }
    }
}