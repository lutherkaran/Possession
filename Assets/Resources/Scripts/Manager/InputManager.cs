using System;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class InputManager : MonoBehaviour
{
    public static InputManager instance { get; private set; }

    public event EventHandler OnGamePaused;

    private PlayerInput playerInput;

    private PlayerController player;
    private Entity controlledEntity;

    private void Awake()
    {
        if (instance != null && instance == this)
        {
            Destroy(gameObject);
        }

        instance = this;

        playerInput = new PlayerInput();

        playerInput.OnFoot.Enable();
        playerInput.OnPossession.Enable();
    }

    public void Start()
    {
        player = PlayerController.instance.GetPlayer();

        PossessionManager.instance.OnPossessed += SetControlledEntity;

        playerInput.OnPossession.Possession.performed += HandlePossessionInput;
        playerInput.OnFoot.MouseInteraction.performed += ctx => CameraManager.instance.GetMouseAim()?.ToggleMouseInteraction();
        playerInput.OnFoot.Attack.performed += ctx => player?.Attack();
        playerInput.OnFoot.Pause.performed += Pause_performed;
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnGamePaused?.Invoke(this, EventArgs.Empty);
    }

    private void SetControlledEntity(object sender, IPossessable controlledEntity)
    {
        this.controlledEntity = controlledEntity.GetPossessedEntity();
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

    private void Update()
    {
        playerInput.OnFoot.Sprint.performed += ctx => controlledEntity.Sprint();
        playerInput.OnFoot.Jump.performed += ctx => controlledEntity.ProcessJump();
    }

    private void FixedUpdate()
    {
        controlledEntity.ProcessMove(playerInput.OnFoot.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        CameraManager.instance.GetMouseAim().ProcessLook(playerInput.OnFoot.Look.ReadValue<Vector2>());
    }

    void OnDestroy()
    {
        PossessionManager.instance.OnPossessed -= SetControlledEntity;
        playerInput.OnPossession.Possession.performed -= HandlePossessionInput;
        playerInput.OnFoot.MouseInteraction.performed -= ctx => CameraManager.instance.GetMouseAim()?.ToggleMouseInteraction();
        playerInput.OnFoot.Attack.performed -= ctx => player?.Attack();
        playerInput.OnFoot.Pause.performed -= Pause_performed;

        playerInput.Dispose();
    }

    public PlayerInput.OnFootActions GetOnFootActions() => playerInput.OnFoot;
    public PlayerInput.OnPossessionActions GetOnPossessionActions() => playerInput.OnPossession;
}