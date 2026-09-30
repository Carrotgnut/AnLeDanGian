using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class SetupPickupPrototype
{
    private const string MenuPath =
        "Tools/An Le Dan Gian/Setup Pickup Prototype";

    [MenuItem(MenuPath)]
    public static void Setup()
    {
        EnsureInventoryManager();
        InteractionUIController uiController = EnsureInteractionUI();
        EnsurePickupPrototype();

        EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene()
        );
        EditorSceneManager.SaveOpenScenes();

        Selection.activeGameObject = uiController.gameObject;
        Debug.Log(
            "Pickup prototype setup complete: InventoryManager, " +
            "InteractionUIController, ReadPanel và clue_001 đã sẵn sàng."
        );
    }

    private static void EnsureInventoryManager()
    {
        InventoryManager existing =
            Object.FindObjectOfType<InventoryManager>();

        if (existing != null)
        {
            return;
        }

        GameObject managerObject = new GameObject("InventoryManager");
        Undo.RegisterCreatedObjectUndo(
            managerObject,
            "Create InventoryManager"
        );
        managerObject.AddComponent<InventoryManager>();
    }

    private static InteractionUIController EnsureInteractionUI()
    {
        InteractionUIController existing =
            Object.FindObjectOfType<InteractionUIController>();

        if (existing != null)
        {
            return existing;
        }

        GameObject interactionCanvas =
            GameObject.Find("InteractionCanvas");

        if (interactionCanvas == null)
        {
            interactionCanvas = CreateCanvas("InteractionCanvas", 10);
        }

        InteractionUIController controller =
            interactionCanvas.AddComponent<InteractionUIController>();

        GameObject prompt = GameObject.Find("InteractionText");
        TMP_Text promptText =
            prompt != null ? prompt.GetComponent<TMP_Text>() : null;

        if (prompt == null)
        {
            promptText = CreateText(
                interactionCanvas.transform,
                "InteractionText",
                "[E] Tương tác",
                28,
                new Vector2(0f, -250f),
                new Vector2(500f, 60f)
            );
            prompt = promptText.gameObject;
        }

        GameObject readCanvas = GameObject.Find("ReadCanvas");
        if (readCanvas == null)
        {
            readCanvas = CreateCanvas("ReadCanvas", 20);
        }

        GameObject readPanel = GameObject.Find("ReadPanel");
        TMP_Text titleText;
        TMP_Text contentText;

        if (readPanel == null)
        {
            readPanel = CreateReadPanel(
                readCanvas.transform,
                out titleText,
                out contentText
            );
        }
        else
        {
            titleText = FindText("ReadTitleText");
            contentText = FindText("ReadContentText");
        }

        SerializedObject serializedController =
            new SerializedObject(controller);
        serializedController.FindProperty("interactionPrompt")
            .objectReferenceValue = prompt;
        serializedController.FindProperty("interactionText")
            .objectReferenceValue = promptText;
        serializedController.FindProperty("readPanel")
            .objectReferenceValue = readPanel;
        serializedController.FindProperty("readTitleText")
            .objectReferenceValue = titleText;
        serializedController.FindProperty("readContentText")
            .objectReferenceValue = contentText;
        serializedController.FindProperty("closeInputDelay")
            .floatValue = 0.2f;
        serializedController.ApplyModifiedPropertiesWithoutUndo();

        readPanel.SetActive(false);
        return controller;
    }

    private static void EnsurePickupPrototype()
    {
        GameObject existing = GameObject.Find("PickupPrototype_Clue_001");
        if (existing != null)
        {
            return;
        }

        const string prefabPath =
            "Assets/Prefabs/Items/CluePickup_001.prefab";
        GameObject prefab =
            AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

        if (prefab == null)
        {
            Debug.LogError(
                "Không tìm thấy prefab kiểm thử tại " + prefabPath
            );
            return;
        }

        GameObject pickup = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        pickup.name = "PickupPrototype_Clue_001";
        Undo.RegisterCreatedObjectUndo(pickup, "Create pickup prototype");

        Camera camera = Camera.main;
        if (camera != null)
        {
            Vector3 position =
                camera.transform.position + camera.transform.forward * 2f;
            position.y = Mathf.Max(0.75f, position.y - 0.45f);
            pickup.transform.position = position;
            pickup.transform.rotation =
                Quaternion.LookRotation(-camera.transform.forward, Vector3.up);
        }
        else
        {
            pickup.transform.position = new Vector3(0f, 1f, -1.5f);
        }
    }

    private static GameObject CreateCanvas(string name, int sortingOrder)
    {
        GameObject canvasObject = new GameObject(
            name,
            typeof(RectTransform),
            typeof(Canvas),
            typeof(CanvasScaler),
            typeof(GraphicRaycaster)
        );
        Undo.RegisterCreatedObjectUndo(canvasObject, "Create " + name);

        Canvas canvas = canvasObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = sortingOrder;

        CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);

        return canvasObject;
    }

    private static GameObject CreateReadPanel(
        Transform parent,
        out TMP_Text titleText,
        out TMP_Text contentText)
    {
        GameObject panel = new GameObject(
            "ReadPanel",
            typeof(RectTransform),
            typeof(CanvasRenderer),
            typeof(Image)
        );
        panel.transform.SetParent(parent, false);

        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.sizeDelta = new Vector2(760f, 460f);

        Image background = panel.GetComponent<Image>();
        background.color = new Color(0.05f, 0.04f, 0.035f, 0.96f);

        titleText = CreateText(
            panel.transform,
            "ReadTitleText",
            "Tiêu đề vật phẩm",
            38,
            new Vector2(0f, 160f),
            new Vector2(660f, 70f)
        );
        titleText.fontStyle = FontStyles.Bold;

        contentText = CreateText(
            panel.transform,
            "ReadContentText",
            "Nội dung vật phẩm",
            28,
            new Vector2(0f, 5f),
            new Vector2(650f, 220f)
        );
        contentText.alignment = TextAlignmentOptions.TopLeft;

        TMP_Text closeText = CreateText(
            panel.transform,
            "ReadCloseText",
            "[E] Đóng",
            24,
            new Vector2(0f, -175f),
            new Vector2(300f, 45f)
        );
        closeText.color = new Color(0.85f, 0.78f, 0.58f, 1f);

        return panel;
    }

    private static TMP_Text CreateText(
        Transform parent,
        string name,
        string text,
        float fontSize,
        Vector2 anchoredPosition,
        Vector2 size)
    {
        GameObject textObject = new GameObject(
            name,
            typeof(RectTransform),
            typeof(CanvasRenderer),
            typeof(TextMeshProUGUI)
        );
        textObject.transform.SetParent(parent, false);

        RectTransform rect = textObject.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = anchoredPosition;
        rect.sizeDelta = size;

        TextMeshProUGUI label = textObject.GetComponent<TextMeshProUGUI>();
        label.text = text;
        label.fontSize = fontSize;
        label.color = Color.white;
        label.alignment = TextAlignmentOptions.Center;
        label.enableWordWrapping = true;

        return label;
    }

    private static TMP_Text FindText(string objectName)
    {
        GameObject textObject = GameObject.Find(objectName);
        return textObject != null ? textObject.GetComponent<TMP_Text>() : null;
    }
}

