using UnityEngine;

public class InteractionRaycast : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private Camera playerCamera;

    [Header("Dialogue")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private CuMuoiDialogueManager cuMuoiDialogueManager;

    [Header("Legacy UI (optional)")]
    [Tooltip("Chỉ dùng khi Scene chưa có InteractionUIController.")]
    [SerializeField] private GameObject interactionUI;

    private IInteractable currentInteractable;

    private void Awake()
    {
        FindDialogueManagers();
    }

    private void Update()
    {
        CheckForInteractable();
    }

    private void CheckForInteractable()
    {
        currentInteractable = null;

        InteractionUIController uiController = InteractionUIController.Instance;

        if (IsInteractionBlocked(uiController))
        {
            HidePrompt(uiController);
            return;
        }

        if (playerCamera == null)
        {
            HidePrompt(uiController);
            return;
        }

        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
        {
            currentInteractable =
                hit.collider.GetComponentInParent<IInteractable>();

            if (currentInteractable != null)
            {
                ShowPrompt(uiController);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    currentInteractable.Interact();

                    // Hội thoại hoặc bảng đọc có thể vừa được mở bởi lần
                    // nhấn E này. Ẩn prompt ngay, không chờ frame sau.
                    if (IsInteractionBlocked(uiController))
                    {
                        HidePrompt(uiController);
                    }
                }

                return;
            }
        }

        HidePrompt(uiController);
    }

    private bool IsInteractionBlocked(
        InteractionUIController uiController)
    {
        if (uiController != null && uiController.IsReadPanelOpen)
        {
            return true;
        }

        // Manager có thể được tạo sau Player khi chuyển Scene.
        if (dialogueManager == null && cuMuoiDialogueManager == null)
        {
            FindDialogueManagers();
        }

        if (dialogueManager != null &&
            dialogueManager.IsDialogueActive())
        {
            return true;
        }

        return cuMuoiDialogueManager != null &&
            cuMuoiDialogueManager.IsDialogueActive();
    }

    private void FindDialogueManagers()
    {
        if (dialogueManager == null)
        {
            dialogueManager = FindObjectOfType<DialogueManager>();
        }

        if (cuMuoiDialogueManager == null)
        {
            cuMuoiDialogueManager =
                FindObjectOfType<CuMuoiDialogueManager>();
        }
    }

    private void ShowPrompt(InteractionUIController uiController)
    {
        if (uiController != null)
        {
            uiController.ShowInteractionPrompt();
            return;
        }

        if (interactionUI != null)
        {
            interactionUI.SetActive(true);
        }
    }

    private void HidePrompt(InteractionUIController uiController)
    {
        if (uiController != null)
        {
            uiController.HideInteractionPrompt();
            return;
        }

        if (interactionUI != null)
        {
            interactionUI.SetActive(false);
        }
    }
}
