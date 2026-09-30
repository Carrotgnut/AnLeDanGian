using TMPro;
using UnityEngine;

public class InteractionUIController : MonoBehaviour
{
    public static InteractionUIController Instance { get; private set; }

    [Header("Prompt E")]
    [SerializeField] private GameObject interactionPrompt;
    [SerializeField] private TMP_Text interactionText;

    [Header("Read Panel")]
    [SerializeField] private GameObject readPanel;
    [SerializeField] private TMP_Text readTitleText;
    [SerializeField] private TMP_Text readContentText;

    [Header("Input")]
    [SerializeField] private float closeInputDelay = 0.2f;

    private bool isReadPanelOpen;
    private float readPanelOpenedTime;
    private int readPanelOpenedFrame = -1;

    public bool IsReadPanelOpen
    {
        get { return isReadPanelOpen; }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        HideInteractionPrompt();
        CloseReadPanel();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Update()
    {
        if (!isReadPanelOpen)
        {
            return;
        }

        // Không dùng lại lần nhấn E vừa nhặt vật phẩm để đóng panel.
        if (Time.frameCount == readPanelOpenedFrame ||
            Time.unscaledTime < readPanelOpenedTime + closeInputDelay)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            CloseReadPanel();
        }
    }

    public void ShowInteractionPrompt(
        string message = "[E] Tương tác")
    {
        if (isReadPanelOpen)
        {
            HideInteractionPrompt();
            return;
        }

        if (interactionText != null)
        {
            interactionText.text = message;
        }

        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(true);
        }
    }

    public void HideInteractionPrompt()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }

    public void OpenReadPanel(InventoryItem item)
    {
        if (item == null)
        {
            return;
        }

        isReadPanelOpen = true;
        readPanelOpenedFrame = Time.frameCount;
        readPanelOpenedTime = Time.unscaledTime;

        HideInteractionPrompt();

        if (readTitleText != null)
        {
            readTitleText.text = item.displayName;
        }

        if (readContentText != null)
        {
            readContentText.text = item.readText;
        }

        if (readPanel != null)
        {
            readPanel.SetActive(true);
        }
    }

    public void CloseReadPanel()
    {
        isReadPanelOpen = false;
        readPanelOpenedFrame = -1;

        if (readPanel != null)
        {
            readPanel.SetActive(false);
        }
    }
}
