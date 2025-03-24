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
        if (player != null)
        {
            PossessionManager.Instance.ToPossess(player.gameObject);
            OnPossessionActions.Possession.performed += Possession_performed1;
            OnFootActions.MouseInteraction.performed += ctx => CameraManager.instance.GetMouseAim()?.MouseInteraction();
            OnFootActions.Attack.performed += ctx => player.Attack();
        }
    }

    private void Possession_performed1(UnityEngine.InputSystem.InputAction.CallbackContext obj)
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

    private void Possession_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        throw new System.NotImplementedException();
    }

    private void Update()
    {
        controlledEntity = PossessionManager.Instance.GetCurrentPossessable().GetEntity();
        if (controlledEntity != null)
        {
            OnFootActions.Sprint.performed += ctx => controlledEntity.Sprint();
            OnFootActions.Jump.performed += ctx => controlledEntity.ProcessJump();
        }
    }

    private void FixedUpdate()
    {
        if (controlledEntity != null)
            controlledEntity.ProcessMove(OnFootActions.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        if (controlledEntity != null)
        {
            CameraManager.instance.GetMouseAim().ProcessLook(OnFootActions.Look.ReadValue<Vector2>());
        }
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
    }
}
// Whatever the entity is currently possessed, they should be able to move/jump/attack etc.