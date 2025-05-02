using UnityEngine;

public class MouseAim
{
    [Header("Mouse Controls")]
    [SerializeField] private float xRotation = 0f;
    [SerializeField] private bool MouseVisible = false;
    [SerializeField] private float xSensitivity = 30f;
    [SerializeField] private float ySensitivity = 30f;

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        if (!MouseVisible)
        {
            CameraManager.instance.cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            PossessionManager.Instance.GetCurrentPossessable().GetEntity().gameObject.transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
        }
    }

    public void MouseInteraction()
    {
        MouseVisible = !MouseVisible;

        if (!MouseVisible)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

}
