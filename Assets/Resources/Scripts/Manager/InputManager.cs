using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class InputManager : MonoBehaviour
{
    public PlayerInput playerInput;
    public PlayerInput.OnPossessionActions OnPossessionActions;
    public static PlayerInput.OnFootActions OnFootActions;

    private PlayerController player;
    private Entity controlledEntity;

    private void Awake()
    {
        playerInput = new PlayerInput();
        player = GetComponent<PlayerController>();
        OnFootActions = playerInput.OnFoot;
        OnPossessionActions = playerInput.OnPossession;
    }

    public void Start()
    {
        PossessionManager.Instance.OnPossessed += SetControlledEntity;

        PossessionManager.Instance.ToPossess(player.gameObject);
        OnPossessionActions.Possession.performed += HandlePossessionInput;
        OnFootActions.MouseInteraction.performed += ctx => CameraManager.instance.GetMouseAim()?.MouseInteraction();
        OnFootActions.Attack.performed += ctx => player.Attack();

    }

    private void SetControlledEntity(object sender, GameObject controlledEntity)
    {
        this.controlledEntity = controlledEntity.GetComponent<Entity>();
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
}