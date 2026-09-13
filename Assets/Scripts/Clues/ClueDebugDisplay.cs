using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueDebugDisplay : MonoBehaviour
{
    private void Update()
    {
        if (ClueManager.Instance == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log(
                "Clue bắt buộc đã tìm: "
                + ClueManager.Instance.DiscoveredRequiredClueCount()
                + "/"
                + ClueManager.Instance.RequiredClueCount()
            );

            Debug.Log(
                "Đủ điều kiện sự thật: "
                + ClueManager.Instance.HasEnoughCluesForTruth()
            );
        }
    }
}