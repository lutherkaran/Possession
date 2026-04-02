using UnityEngine;

public class Keypad : Interactable
{
    [SerializeField]
    private GameObject door;
    private bool isDoorOpened = false;

    protected override void Interact()
    {
        isDoorOpened = !isDoorOpened;
        door.GetComponent<Animator>().SetBool("IsOpen", isDoorOpened);
    }
}
