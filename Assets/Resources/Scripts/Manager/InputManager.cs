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

    public void Initialize()
    {
        playerInput = new PlayerInput();

        playerInput.OnFoot.Enable();
        playerInput.OnPossession.Enable();
    }

    public void PostInitialize()
    {
        player = PlayerManager.instance.GetPlayer();

        playerInput.OnPossession.Possession.performed += HandlePossessionInput;
        playerInput.OnFoot.MouseInteraction.performed += ctx => CameraManager.instance.GetMouseAim()?.ToggleMouseInteraction();
        playerInput.OnFoot.Attack.performed += ctx => player?.Attack();
        playerInput.OnFoot.Pause.performed += Pause_performed;
    }

    public void Refresh(float deltaTime)
    {
        playerInput.OnFoot.Sprint.performed += ctx => PossessionManager.instance.GetCurrentPossessable().GetPossessedEntity().Sprint();
        playerInput.OnFoot.Jump.performed += ctx => PossessionManager.instance.GetCurrentPossessable().GetPossessedEntity().ProcessJump();
    }

    public void PhysicsRefresh(float fixedDeltaTime)
    {
        PossessionManager.instance.GetCurrentPossessable().GetPossessedEntity().ProcessMove(playerInput.OnFoot.Movement.ReadValue<Vector2>());
    }

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
        playerInput.OnPossession.Possession.performed -= HandlePossessionInput;
        playerInput.OnFoot.MouseInteraction.performed -= ctx => CameraManager.instance.GetMouseAim()?.ToggleMouseInteraction();
        playerInput.OnFoot.Attack.performed -= ctx => player?.Attack();
        playerInput.OnFoot.Pause.performed -= Pause_performed;

        playerInput.Dispose();
        Instance = null;
    }

    public PlayerInput.OnFootActions GetOnFootActions() => playerInput.OnFoot;
    public PlayerInput.OnPossessionActions GetOnPossessionActions() => playerInput.OnPossession;
}