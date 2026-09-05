using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Đã tương tác với: " + gameObject.name);
    }
}
