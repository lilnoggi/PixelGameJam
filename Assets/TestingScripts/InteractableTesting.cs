using UnityEngine;

/// <summary>
/// Example interactable object for testing interaction.
/// Implements IInteractable interface.
/// </summary>
public class InteractableTesting : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Interacted!");
    }
}
