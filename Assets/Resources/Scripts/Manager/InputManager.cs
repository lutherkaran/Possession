using System;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public PlayerInput playerInput;

    private PlayerInput.OnFootActions OnFootActions;
    private PlayerInput.OnPossessionActions OnPossessionActions;

    private PlayerController player;
    private Entity controlledEntity;

    public event EventHandler OnGamePaused;

    private void Awake()
    {
        Instance = this;

        playerInput = new PlayerInput();
        OnFootActions = playerInput.OnFoot;
        OnPossessionActions = playerInput.OnPossession;
    }

    public void Start()
    {
        player = GetComponent<PlayerController>();

        PossessionManager.Instance.OnPossessed += SetControlledEntity;
        PossessionManager.Instance.ToPossess(player.gameObject);

        OnPossessionActions.Possession.performed += HandlePossessionInput;
        OnFootActions.MouseInteraction.performed += ctx => CameraManager.instance.GetMouseAim()?.ToggleMouseInteraction();
        OnFootActions.Attack.performed += ctx => player.Attack();
        OnFootActions.Pause.performed += Pause_performed;

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
        if (obj.interaction is PressInteraction)
        {
            PossessionManager.Instance.GetCurrentPossession()?.PossessEntities();
        }
        else if (obj.interaction is HoldInteraction)
        {
            PossessionManager.Instance.GetCurrentPossession()?.RepossessPlayer(player.gameObject);
        }
    }

    private void Update()
    {
        OnFootActions.Sprint.performed += ctx => controlledEntity.Sprint();
        OnFootActions.Jump.performed += ctx => controlledEntity.ProcessJump();
    }

    private void FixedUpdate()
    {
        controlledEntity.ProcessMove(OnFootActions.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        CameraManager.instance.GetMouseAim().ProcessLook(OnFootActions.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        OnFootActions.Enable();
        OnPossessionActions.Enable();
    }

    private void OnDisable()
    {
        OnFootActions.Disable();
        OnPossessionActions.Disable();
        UnsubscribeEvents();
    }

    private void UnsubscribeEvents()
    {
        PossessionManager.Instance.OnPossessed -= SetControlledEntity;
    }

    public PlayerInput.OnFootActions GetOnFootActions() => OnFootActions;
    public PlayerInput.OnPossessionActions GetOnPossessionActions() => OnPossessionActions;
}