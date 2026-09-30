using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
    [Header("Item Data")]
    [SerializeField] private string itemId = "clue_001";
    [SerializeField] private string displayName = "Mảnh giấy ghi giờ giao nhận";
    [SerializeField] private ItemType itemType = ItemType.Clue;

    [TextArea(3, 10)]
    [SerializeField]
    private string readText =
        "6 giờ sáng. 6 giờ tối. Không được trễ.";

    [Header("Options")]
    [SerializeField] private bool openReadPanelAfterPickup = true;
    [SerializeField] private bool disableInsteadOfDestroy = true;

    private bool hasBeenPickedUp;

    public void Interact()
    {
        if (hasBeenPickedUp)
        {
            return;
        }

        if (InventoryManager.Instance == null)
        {
            Debug.LogError(
                "PickupItem: Chưa có InventoryManager trong Scene."
            );

            return;
        }

        InventoryItem item = new InventoryItem(
            itemId,
            displayName,
            itemType,
            readText
        );

        bool added = InventoryManager.Instance.AddItem(item);

        if (!added)
        {
            return;
        }

        hasBeenPickedUp = true;

        RegisterClueIfNecessary();

        HideInteractionPrompt();

        if (disableInsteadOfDestroy)
        {
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }

        if (openReadPanelAfterPickup)
        {
            OpenReadPanel(item);
        }
    }

    private void RegisterClueIfNecessary()
    {
        if (itemType != ItemType.Clue)
        {
            return;
        }

        if (ClueManager.Instance == null)
        {
            Debug.LogWarning(
                "PickupItem: Không tìm thấy ClueManager. "
                + "Item vẫn được lưu vào Inventory."
            );

            return;
        }

        ClueManager.Instance.TryDiscover(itemId, false);
    }

    private void HideInteractionPrompt()
    {
        if (InteractionUIController.Instance != null)
        {
            InteractionUIController.Instance.HideInteractionPrompt();
        }
    }

    private void OpenReadPanel(InventoryItem item)
    {
        if (InteractionUIController.Instance != null)
        {
            InteractionUIController.Instance.OpenReadPanel(item);
        }
    }
}


