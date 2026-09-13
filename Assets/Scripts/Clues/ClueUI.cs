using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClueUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text bodyText;
    [SerializeField] private TMP_Text counterText;

    private float openedTime;

    private void Awake()
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }

    private void Update()
    {
        if (panel == null || !panel.activeSelf)
        {
            return;
        }

        // Tránh việc phím E vừa mở clue đã lập tức đóng clue.
        if (Time.time < openedTime + 0.2f)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Close();
        }
    }

    public void ShowNewClue(ClueData clue, int discoveredCount, int requiredCount)
    {
        if (panel == null)
        {
            return;
        }

        titleText.text = clue.title;
        bodyText.text = clue.text;
        counterText.text = "Bằng chứng đã tìm thấy: "
            + discoveredCount + "/" + requiredCount;

        panel.SetActive(true);
        openedTime = Time.time;
    }

    public void ShowAlreadyDiscovered(
        ClueData clue,
        int discoveredCount,
        int requiredCount)
    {
        if (panel == null)
        {
            return;
        }

        titleText.text = clue.title + " (đã ghi nhận)";
        bodyText.text = clue.text;
        counterText.text = "Bằng chứng đã tìm thấy: "
            + discoveredCount + "/" + requiredCount;

        panel.SetActive(true);
        openedTime = Time.time;
    }

    public void Close()
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }
}