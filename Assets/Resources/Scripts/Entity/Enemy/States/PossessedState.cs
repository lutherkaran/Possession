using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossessedState : BaseState
{
    public override void Enter()
    {
        enemy.Agent.velocity = Vector3.zero;
    }

    public override void Perform()
    {
        if (PossessionManager.Instance.GetCurrentPossessable() != enemy.possessedByPlayer)
        {
            stateMachine.ChangeState(new IdleState());
        }
    }

    public override void Exit()
    {

    }

}
