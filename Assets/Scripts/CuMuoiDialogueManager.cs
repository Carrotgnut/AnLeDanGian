using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
public class CuMuoiDialogueManager : MonoBehaviour
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

    [Header("UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text speakerText;
    [SerializeField] private TMP_Text dialogueText;

    [Header("Cooldown")]
    [SerializeField] private float reinteractDelay = 1f;

    private Dictionary<string, DialogueLine> dialogueDictionary;
    private DialogueLine currentLine;
    private bool isDialogueActive;
    private int dialogueOpenedFrame = -1;
    private float nextAllowedStartTime;

    public int LastDialogueEndedFrame { get; private set; } = -1;

    private void Awake()
    {
        dialogueDictionary =
            new Dictionary<string, DialogueLine>();

        HideDialogue();
    }

    private void Update()
    {
        if (!isDialogueActive)
        {
            return;
        }

        if (Time.frameCount == dialogueOpenedFrame)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ShowNextLine();
        }
    }

    public bool StartDialogue(
        TextAsset dialogueJson,
        string startId)
    {
        if (isDialogueActive)
        {
            return false;
        }

        if (Time.time < nextAllowedStartTime)
        {
            return false;
        }

        if (LastDialogueEndedFrame == Time.frameCount)
        {
            return false;
        }

        if (!LoadDialogue(dialogueJson))
        {
            return false;
        }

        if (!dialogueDictionary.TryGetValue(
            startId,
            out currentLine))
        {
            Debug.LogError(
                "CuMuoiDialogueManager: Không tìm thấy ID "
                + startId
                + " trong "
                + dialogueJson.name
            );

            return false;
        }

        isDialogueActive = true;
        dialogueOpenedFrame = Time.frameCount;
        LastDialogueEndedFrame = -1;

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
        }

        DisplayCurrentLine();

        return true;
    }

    private bool LoadDialogue(TextAsset dialogueJson)
    {
        dialogueDictionary.Clear();

        if (dialogueJson == null)
        {
            Debug.LogError(
                "CuMuoiDialogueManager: Chưa gán file JSON."
            );

            return false;
        }

        DialogueFile file =
            JsonUtility.FromJson<DialogueFile>(
                dialogueJson.text
            );

        if (file == null ||
            file.dialogues == null ||
            file.dialogues.Length == 0)
        {
            Debug.LogError(
                "CuMuoiDialogueManager: JSON không hợp lệ: "
                + dialogueJson.name
            );

            return false;
        }

        foreach (DialogueLine line in file.dialogues)
        {
            if (line == null ||
                string.IsNullOrEmpty(line.id))
            {
                continue;
            }

            dialogueDictionary[line.id] = line;
        }

        return dialogueDictionary.Count > 0;
    }

    private void DisplayCurrentLine()
    {
        if (currentLine == null)
        {
            return;
        }

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
        if (currentLine == null ||
            string.IsNullOrEmpty(currentLine.nextId))
        {
            EndDialogue();
            return;
        }

        string nextId = currentLine.nextId;

        if (!dialogueDictionary.TryGetValue(
            nextId,
            out currentLine))
        {
            Debug.LogError(
                "CuMuoiDialogueManager: Không tìm thấy ID tiếp theo "
                + nextId
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
        LastDialogueEndedFrame = Time.frameCount;
        nextAllowedStartTime = Time.time + reinteractDelay;

        HideDialogue();

        onDialogueEnded?.Invoke();
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