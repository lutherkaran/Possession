using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions OnFootActions;
    private Player player;

    private void Awake()
    {
        playerInput = new PlayerInput();
        OnFootActions = playerInput.OnFoot;
        player = GetComponent<Player>();
        OnFootActions.Jump.performed += ctx => player.ProcessJump();
        OnFootActions.Possession.performed += ctx => player.PossessEntities();
        OnFootActions.UnPossession.performed += ctx => player.UnPossessed();
    }

    private void FixedUpdate()
    {
        if (player.currentPossession != null)
        {
            player.ProcessMove(OnFootActions.Movement.ReadValue<Vector2>());
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
