using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Camera cam;
    [SerializeField]
    float distance = 3f;
    [SerializeField]
    LayerMask layer;
    private PlayerUI playerUI;
    private InputManager playerInput;
    private void Start()
    {
        cam = this.GetComponent<PlayerLook>().cam;
        playerUI = this.GetComponent<PlayerUI>();
        playerInput = this.GetComponent<InputManager>();
    }

    private void Update()
    {
        playerUI.UpdateText(string.Empty);
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction, (Color.red));
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, distance, layer))
        {
            Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                playerUI.UpdateText(hitInfo.collider.GetComponent<Interactable>().PromptMessage);

                if (playerInput.OnFootActions.Interact.triggered)
                {
                    interactable.BaseInteract();
                }
            }
        }
    }
}
