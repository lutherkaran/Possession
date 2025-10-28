using UnityEngine;

[System.Serializable]
public class MouseAim
{
    [Header("Mouse Controls")]
    [SerializeField] private float xRotation = 0f;
    [SerializeField] private float xSensitivity = 10f;
    [SerializeField] private float ySensitivity = 10f;
    [SerializeField] private bool MouseVisible = false;

    public void ProcessLook(Vector2 input, float lateDeltaTime)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        xRotation -= (mouseY * lateDeltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        if (!MouseVisible)
        {
            CameraManager.instance.myCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            PossessionManager.instance.GetCurrentPossessable().GetPossessedEntity().transform.Rotate(Vector3.up * (mouseX * lateDeltaTime) * xSensitivity);
        }
    }

    public void ToggleMouseInteraction()
    {
        MouseVisible = !MouseVisible;

        if (MouseVisible)
        {
            Cursor.lockState = CursorLockMode.None;
            ShowMouse();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            HideMouse();
        }
    }

    public void OnFocus()
    {
        HideMouse();
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ShowMouse()
    {
        Cursor.visible = true;
    }

    public void HideMouse()
    {
        Cursor.visible = false;
    }
}
