using Codice.CM.Common;
using log4net.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static PlayerInput;

public class PossessedState : BaseState
{
    private readonly float WalkSpeed = 10f;
    private Vector3 moveDirection;

    public override void Enter()
    {
        enemy.Agent.velocity = Vector3.zero;
        enemy.Agent.isStopped = true;
    }

    public override void Perform()
    {
        moveDirection.x = InputManager.OnFootActions.Movement.ReadValue<Vector2>().x;
        moveDirection.z = InputManager.OnFootActions.Movement.ReadValue<Vector2>().y;
        moveDirection.y = 0;

        if (PossessionManager.Instance.GetCurrentPossessable() == enemy.possessedByPlayer)
            enemy.transform.Translate(moveDirection * WalkSpeed * Time.deltaTime);
    }

    public override void Exit()
    {

    }

}
