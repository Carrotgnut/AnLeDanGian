using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private readonly Dictionary<string, InventoryItem> items =
        new Dictionary<string, InventoryItem>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool AddItem(InventoryItem item)
    {
        if (item == null)
        {
            Debug.LogError("InventoryManager: item bị null.");
            return false;
        }

        if (string.IsNullOrEmpty(item.itemId))
        {
            Debug.LogError("InventoryManager: item chưa có itemId.");
            return false;
        }

        if (items.ContainsKey(item.itemId))
        {
            Debug.LogWarning(
                "InventoryManager: Vật phẩm đã tồn tại: "
                + item.itemId
            );

            return false;
        }

        items.Add(item.itemId, item);

        Debug.Log(
            "InventoryManager: Đã thêm vật phẩm: "
            + item.itemId
        );

        return true;
    }

    public bool HasItem(string itemId)
    {
        if (string.IsNullOrEmpty(itemId))
        {
            return false;
        }

        return items.ContainsKey(itemId);
    }

    public bool TryGetItem(
        string itemId,
        out InventoryItem item)
    {
        return items.TryGetValue(itemId, out item);
    }

    public List<InventoryItem> GetAllItems()
    {
        return new List<InventoryItem>(items.Values);
    }

    public int GetItemCount()
    {
        return items.Count;
    }
}
