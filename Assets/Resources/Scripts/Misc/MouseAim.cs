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

    // Camera pitch + target-locker refresh only -- called from LateUpdate (via
    // InputManager.LateRefresh) for maximum camera responsiveness. This never touches a
    // Rigidbody, so LateUpdate timing is fine here.
    public void ProcessLook(Vector2 input, float lateDeltaTime)
    {
        HandleCameraPitch(input.y, lateDeltaTime);
        HandleTargetLocker();
    }

    // Entity yaw -- called from FixedUpdate (via InputManager.PhysicsRefresh) instead of
    // LateUpdate. Rotation goes through the possessed entity's Rigidbody (MoveRotation), and
    // mixing Rigidbody-API calls across two different update frequencies (Update/LateUpdate vs
    // FixedUpdate) is what caused visible jitter on movement + rotation -- Unity's own guidance
    // is that all Rigidbody moves belong in FixedUpdate. This keeps rotation on the same clock
    // as MovePosition.
    public void ProcessEntityYaw(float mouseX, float fixedDeltaTime)
    {
        if (mouseVisible) return;

        Entity possessedEntity = PossessionManager.instance.GetCurrentPossessable().GetPossessedEntity();
        float yaw = (mouseX * fixedDeltaTime) * xSensitivity;
        Rigidbody entityRb = possessedEntity.GetRigidBody();

        if (entityRb != null && !entityRb.isKinematic)
        {
            entityRb.MoveRotation(entityRb.rotation * Quaternion.Euler(0f, yaw, 0f));
        }
        else
        {
            possessedEntity.transform.Rotate(Vector3.up * yaw);
        }
    }

    private void HandleTargetLocker()
    {
        targetLocker.Refresh();
    }

    private void HandleCameraPitch(float mouseY, float lateDeltaTime)
    {
        xRotation -= (mouseY * lateDeltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 13.5f);

        if (!mouseVisible)
        {
            CameraManager.instance.myCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
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