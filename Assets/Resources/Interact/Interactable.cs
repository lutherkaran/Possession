using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool useEvents;

    [SerializeField]
    public string PromptMessage;

    public void BaseInteract()
    {
        if (useEvents)
        {
            GetComponent<InteractionEvents>().OnInteract?.Invoke();
        }

        Interact();
    }

    protected virtual void Interact()
    {

    }
}
