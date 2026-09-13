using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

[Serializable]
public class ClueData
{
    public string id;
    public string title;
    public string text;
    public int chapter;
    public bool requiredForTruth;
}

[Serializable]
public class ClueDatabase
{
    public ClueData[] clues;
}