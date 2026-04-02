using UnityEngine;

public class MouseAim : MonoBehaviour
{
    [Header("Mouse Controls")]
    [SerializeField] private float xRotation = 0f;
    [SerializeField] private float xSensitivity = 45f;
    [SerializeField] private float ySensitivity = 45f;
    [SerializeField] private bool mouseVisible = false;

    [SerializeField] private TargetLocker targetLocker;

    public void InitializeTargetLocker()
    {
        targetLocker.Initialize();
    }

    public void ProcessLook(Vector2 input, float lateDeltaTime)
    {
        HandleLook(input, lateDeltaTime);
        HandleTargetLocker();
    }

    private void HandleTargetLocker()
    {
        targetLocker.Refresh();
    }

    private void HandleLook(Vector2 input, float lateDeltaTime)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        xRotation -= (mouseY * lateDeltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 13.5f);

        if (!mouseVisible)
        {
            CameraManager.instance.myCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            PossessionManager.instance.GetCurrentPossessable().GetPossessedEntity().transform.Rotate(Vector3.up * (mouseX * lateDeltaTime) * xSensitivity);
        }
    }

    public void ToggleMouseInteraction()
    {
        mouseVisible = !mouseVisible;

        if (mouseVisible)
        {
            Cursor.lockState = CursorLockMode.None;
            ToggleVisibility(true);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            ToggleVisibility(false);
        }
    }

    public void OnFocus()
    {
        ToggleVisibility(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ToggleVisibility(bool visibility)
    {
        Cursor.visible = visibility;
    }

    public TargetLocker GetTargetLocker() => targetLocker;
}
