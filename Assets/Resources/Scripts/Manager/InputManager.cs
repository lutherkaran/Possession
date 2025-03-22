using UnityEngine;

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
            PossessionManager.Instance.ToPossess(player);
            OnPossessionActions.Possession.performed += ctx => PossessionManager.Instance.GetCurrentPossession()?.PossessEntities();
            OnFootActions.MouseInteraction.performed += ctx => CameraManager.instance?.MouseInteraction();
            OnFootActions.Attack.performed += ctx => player.Attack();
        }
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
            CameraManager.instance.ProcessLook(OnFootActions.Look.ReadValue<Vector2>());
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