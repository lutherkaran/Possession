using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keypad : Interactable
{
    [SerializeField]
    private GameObject door;
    private bool isDoorOpened = false;

    protected override void Interact()
    {
        isDoorOpened = !isDoorOpened;
        Debug.Log("Interacted with... " + gameObject.name + isDoorOpened);
        door.GetComponent<Animator>().SetBool("IsOpen", isDoorOpened);
    }
}
