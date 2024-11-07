using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.OnFootActions OnFootActions;
    private PlayerController player; 
    private Entity controlledEntity;

    private void Awake()
    {
        playerInput = new PlayerInput();
        player = GetComponent<PlayerController>();
        OnFootActions = playerInput.OnFoot;

        if (player != null)
        {
            OnFootActions.Possession.performed += ctx => player.PossessEntities();
            OnFootActions.MouseInteraction.performed += ctx => CameraManager.instance.MouseInteraction();
        }
    }

    private void Update()
    {
        controlledEntity = PossessionManager.currentlyPossessed.GetEntity();
        if (controlledEntity != null)
        {
            OnFootActions.Sprint.performed += ctx => controlledEntity.Sprint();
            OnFootActions.Jump.performed += ctx => controlledEntity.ProcessJump();
        }
    }

    private void FixedUpdate()
    {
        controlledEntity = PossessionManager.currentlyPossessed.GetEntity();
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
    }

    private void OnDisable()
    {
        OnFootActions.Sprint.performed -= ctx => controlledEntity.Sprint();
        OnFootActions.Jump.performed -= ctx => controlledEntity.ProcessJump();
        OnFootActions.Disable();
    }
}
// Whatever the entity is currently possessed, they should be able to move/jump/attack etc.