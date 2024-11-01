using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.OnFootActions OnFootActions;
    private PlayerController player;
    private PlayerLook look;
    private Entity entity;

    private void Awake()
    {
        playerInput = new PlayerInput();
        player = GetComponent<PlayerController>();
        look = GetComponent<PlayerLook>();
        entity = GetComponent<Entity>();
        OnFootActions = playerInput.OnFoot;

        if (entity != null)
        {
            OnFootActions.Sprint.performed += ctx => entity.Sprint();
            OnFootActions.Jump.performed += ctx => entity.ProcessJump();
            OnFootActions.MouseInteraction.performed += ctx => look.MouseInteraction();

            OnFootActions.Possession.performed += ctx => ((PlayerController)entity).PossessEntities();
        }
    }

    private void FixedUpdate()
    {
        entity.ProcessMove(OnFootActions.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        if (player.currentPossession != null)
        {
            look.ProcessLook(OnFootActions.Look.ReadValue<Vector2>());
        }
    }

    private void OnEnable()
    {
        OnFootActions.Enable();
    }

    private void OnDisable()
    {
        OnFootActions.Disable();
    }
}
