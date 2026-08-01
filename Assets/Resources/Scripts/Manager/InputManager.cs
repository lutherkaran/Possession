using System;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class InputManager : IManagable
{
    private static InputManager Instance;
    public static InputManager instance { get { return Instance == null ? Instance = new InputManager() : Instance; } }

    public event EventHandler OnGamePaused;

    private PlayerInput playerInput;
    private PlayerController player;

    private Vector2 moveDir = Vector2.zero;

    public void Initialize()
    {
        playerInput = new PlayerInput();

        playerInput.OnFoot.Enable();
    }

    public void PostInitialize()
    {
        player = PlayerManager.instance.GetPlayer();
        HandleInput();
    }

    private void HandleInput()
    {
        playerInput.OnFoot.Possession.performed += HandlePossessionInput;

        playerInput.OnFoot.MouseInteraction.performed += ctx => CameraManager.instance.GetMouseAim()?.ToggleMouseInteraction();

        playerInput.OnFoot.Sprint.performed += ctx => PossessionManager.instance.GetCurrentPossessable().GetPossessedEntity().ToggleSprint();

        playerInput.OnFoot.Pause.performed += Pause_performed;
    }

    public void Refresh(float deltaTime)
    {

    }

    // Physics-timed movement AND rotation for whichever entity is currently possessed.
    // Both MovePosition (movement) and MoveRotation (yaw, via MouseAim.ProcessEntityYaw) now
    // run here, on the same fixed clock -- previously yaw was applied from LateUpdate while
    // movement was applied from FixedUpdate, and mixing Rigidbody-API calls across two
    // different update frequencies caused visible jitter.
    public void PhysicsRefresh(float fixedDeltaTime)
    {
        moveDir = playerInput.OnFoot.Movement.ReadValue<Vector2>().normalized;

        var possessable = PossessionManager.instance.GetCurrentPossessable();
        if (possessable == null) return;

        possessable.GetPossessedEntity().MoveWhenPossessed(moveDir);

        float mouseX = playerInput.OnFoot.Look.ReadValue<Vector2>().x;
        CameraManager.instance.GetMouseAim().ProcessEntityYaw(mouseX, fixedDeltaTime);
    }

    // Camera pitch + target-locker refresh only now -- see MouseAim.ProcessLook.
    public void LateRefresh(float deltaTime)
    {
        CameraManager.instance.GetMouseAim().ProcessLook(playerInput.OnFoot.Look.ReadValue<Vector2>(), deltaTime);
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnGamePaused?.Invoke(this, EventArgs.Empty);
    }

    private void HandlePossessionInput(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (EntityPossessionCooldownUI.Instance.GetCoolingDown()) return; // if can't possess then return

        if (obj.interaction is PressInteraction)
        {
            PossessionManager.instance.GetCurrentPossession()?.PossessEntities();
        }
        else if (obj.interaction is HoldInteraction)
        {
            PossessionManager.instance.GetCurrentPossession()?.RepossessPlayer(player.gameObject);
        }
    }

    public void OnDemolish()
    {
        playerInput.OnFoot.Possession.performed -= HandlePossessionInput;
        playerInput.OnFoot.MouseInteraction.performed -= ctx => CameraManager.instance.GetMouseAim()?.ToggleMouseInteraction();
        playerInput.OnFoot.Pause.performed -= Pause_performed;

        playerInput.OnFoot.Disable();
        playerInput.Dispose();
        Instance = null;
    }

    public PlayerInput.OnFootActions GetOnFootActions() => playerInput.OnFoot;
    public Vector2 GetMoveDirection() => moveDir;
}