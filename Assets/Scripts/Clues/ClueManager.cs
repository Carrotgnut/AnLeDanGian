using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueManager : MonoBehaviour
{
    public static ClueManager Instance { get; private set; }

    [Header("Data")]
    [SerializeField] private TextAsset cluesJson;

    [Header("UI")]
    [SerializeField] private ClueUI clueUI;

    private ClueDatabase database;
    private readonly List<string> discoveredClueIds = new List<string>();

    public int DiscoveredCount
    {
        get { return discoveredClueIds.Count; }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadDatabase();
    }

    private void LoadDatabase()
    {
        if (cluesJson == null)
        {
            Debug.LogError("ClueManager: Chưa gán file clues.json.");
            return;
        }

        database = JsonUtility.FromJson<ClueDatabase>(cluesJson.text);

        if (database == null || database.clues == null)
        {
            Debug.LogError("ClueManager: clues.json không đúng định dạng.");
        }
    }

    public void TryDiscover(string clueId)
    {
        if (database == null || database.clues == null)
        {
            Debug.LogError("ClueManager: Database chưa được nạp.");
            return;
        }

        ClueData clue = FindClue(clueId);

        if (clue == null)
        {
            Debug.LogError("ClueManager: Không tìm thấy clueId: " + clueId);
            return;
        }

        if (discoveredClueIds.Contains(clueId))
        {
            Debug.Log("Clue đã được tìm trước đó: " + clueId);

            if (clueUI != null)
            {
                clueUI.ShowAlreadyDiscovered(clue, DiscoveredCount, RequiredClueCount());
            }

            return;
        }

        discoveredClueIds.Add(clueId);

        Debug.Log("Đã phát hiện clue: " + clueId);

        if (clueUI != null)
        {
            clueUI.ShowNewClue(clue, DiscoveredCount, RequiredClueCount());
        }
    }

    private ClueData FindClue(string clueId)
    {
        foreach (ClueData clue in database.clues)
        {
            if (clue.id == clueId)
            {
                return clue;
            }
        }

        return null;
    }

    public bool HasDiscovered(string clueId)
    {
        return discoveredClueIds.Contains(clueId);
    }

    public int RequiredClueCount()
    {
        int count = 0;

        if (database == null || database.clues == null)
        {
            return count;
        }

        foreach (ClueData clue in database.clues)
        {
            if (clue.requiredForTruth)
            {
                count++;
            }
        }

        return count;
    }

    public int DiscoveredRequiredClueCount()
    {
        int count = 0;

        if (database == null || database.clues == null)
        {
            return count;
        }

        foreach (ClueData clue in database.clues)
        {
            if (clue.requiredForTruth && discoveredClueIds.Contains(clue.id))
            {
                count++;
            }
        }

        return count;
    }

    public bool HasEnoughCluesForTruth()
    {
        return DiscoveredRequiredClueCount() >= RequiredClueCount();
    }
}