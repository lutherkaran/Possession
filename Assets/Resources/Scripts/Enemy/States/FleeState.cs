using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState : BaseState
{
    Vector3 fleeDirection = Vector3.zero;

    public override void Enter()
    {
    }

    public override void Perform()
    {
        if (enemy.IsSafe() & !enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new PatrolState());
            enemy.Agent.speed = 2;
        }
        else
        {
            enemy.Agent.speed = 4;
            fleeDirection = (enemy.transform.position - enemy.Player.transform.position).normalized;
            enemy.Agent.SetDestination(enemy.transform.position + fleeDirection * 10f);
            // Not Safe // Can't runaway // just attack and die
        }
    }

    public override void Exit()
    {

    }
}
