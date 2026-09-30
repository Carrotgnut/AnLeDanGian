using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum ItemType
{
    Clue,
    Readable,
    KeyItem
}

[Serializable]
public class InventoryItem
{
    public string itemId;
    public string displayName;
    public ItemType itemType;
    [TextArea(3, 10)]
    public string readText;

    public InventoryItem(
        string itemId,
        string displayName,
        ItemType itemType,
        string readText)
    {
        this.itemId = itemId;
        this.displayName = displayName;
        this.itemType = itemType;
        this.readText = readText;
    }
}