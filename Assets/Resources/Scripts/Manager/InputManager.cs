using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.OnFootActions OnFootActions;
    private Player player;
    private PlayerLook look;

    private void Awake()
    {
        playerInput = new PlayerInput();
        OnFootActions = playerInput.OnFoot;
        player = GetComponent<Player>();
        look = GetComponent<PlayerLook>();

        OnFootActions.Jump.performed += ctx => player.ProcessJump();
        OnFootActions.Sprint.performed += ctx => player.Sprint();
        OnFootActions.Possession.performed += ctx => player.PossessEntities();
    }

    private void FixedUpdate()
    {
        if (player.currentPossession != null)
        {
            player.ProcessMove(OnFootActions.Movement.ReadValue<Vector2>());
        }
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
