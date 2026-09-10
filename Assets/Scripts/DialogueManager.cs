using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
   
    [Header("Events")]
    [SerializeField] private UnityEvent onDialogueEnded;
    [Serializable]
    public class DialogueLine
    {
        public string id;
        public string speaker;
        public string text;
        public string nextId;
    }

    [Serializable]
    private class DialogueFile
    {
        public DialogueLine[] dialogues;
    }

    [Header("Dialogue Data")]
    [SerializeField] private TextAsset dialogueJson;

    [Header("UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text speakerText;
    [SerializeField] private TMP_Text dialogueText;

    private Dictionary<string, DialogueLine> dialogueDictionary;
    private DialogueLine currentLine;
    private bool isDialogueActive;

    private void Awake()
    {
        LoadDialogue();
        HideDialogue();
    }

    private void Update()
    {
        if (!isDialogueActive)
            return;

        if (Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.Return))
        {
            ShowNextLine();
        }
    }

    private void LoadDialogue()
    {
        if (dialogueJson == null)
        {
            Debug.LogError("DialogueManager: Chưa gán chapter1.json!");
            return;
        }

        DialogueFile file = JsonUtility.FromJson<DialogueFile>(dialogueJson.text);

        if (file == null)
        {
            Debug.LogError("DialogueManager: Không đọc được JSON!");
            return;
        }

        if (file.dialogues == null || file.dialogues.Length == 0)
        {
            Debug.LogError("DialogueManager: JSON không có dialogues!");
            return;
        }

        dialogueDictionary = new Dictionary<string, DialogueLine>();

        foreach (DialogueLine line in file.dialogues)
        {
            if (string.IsNullOrEmpty(line.id))
            {
                Debug.LogWarning("DialogueManager: Có dialogue không có ID.");
                continue;
            }

            dialogueDictionary[line.id] = line;
        }

        Debug.Log(
            "Đã load " +
            dialogueDictionary.Count +
            " dòng thoại từ " +
            dialogueJson.name
        );
    }

    public void StartDialogue(string startId)
    {
        if (dialogueDictionary == null ||
            dialogueDictionary.Count == 0)
        {
            Debug.LogError("DialogueManager: Chưa có dữ liệu thoại!");
            return;
        }

        if (!dialogueDictionary.TryGetValue(startId, out currentLine))
        {
            Debug.LogError(
                "DialogueManager: Không tìm thấy ID " +
                startId
            );
            return;
        }

        isDialogueActive = true;

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
        }

        DisplayCurrentLine();

        Debug.Log("Bắt đầu dialogue: " + startId);
    }

    private void DisplayCurrentLine()
    {
        if (currentLine == null)
            return;

        if (speakerText != null)
        {
            speakerText.text = currentLine.speaker;
        }

        if (dialogueText != null)
        {
            dialogueText.text = currentLine.text;
        }
    }

    private void ShowNextLine()
    {
        if (currentLine == null)
        {
            EndDialogue();
            return;
        }

        if (string.IsNullOrEmpty(currentLine.nextId))
        {
            EndDialogue();
            return;
        }

        if (!dialogueDictionary.TryGetValue(
            currentLine.nextId,
            out currentLine))
        {
            Debug.LogError(
                "Không tìm thấy ID: " +
                currentLine.nextId
            );

            EndDialogue();
            return;
        }

        DisplayCurrentLine();
    }

    private void EndDialogue()
    {
        currentLine = null;
        isDialogueActive = false;

        HideDialogue();

        onDialogueEnded?.Invoke();

        Debug.Log("Kết thúc dialogue.");
    }

    private void HideDialogue()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }
}