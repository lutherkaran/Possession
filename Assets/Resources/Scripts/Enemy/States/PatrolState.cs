using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    public int waypointIndex = 0;
    public float waitTimer;

    public override void Enter()
    {
        enemy.anim.SetBool(Enemy.PATROLLING, true);
        enemy.anim.SetBool(Enemy.ATTACK, false);
    }

    public override void Perform()
    {

        if (PossessionManager.currentlyPossessed != enemy.PlayerPossessed)
        {
            PatrolCycle();

            if (enemy.CanSeePlayer())
            {
                if (enemy.GetHealth() > 30f)
                {
                    stateMachine.ChangeState(new AttackState());
                }
                else
                {
                    stateMachine.ChangeState(new FleeState());
                }
            }

            //stateMachine.ChangeState(new IdleState());
        }

    }

    public override void Exit()
    {
        waitTimer = 0;
        waypointIndex = 0;
    }

    public void PatrolCycle()
    {
        if (enemy.Agent.remainingDistance < 0.2f)
        {
            stateMachine.ChangeState(new PatrolState());
            enemy.Agent.SetDestination(enemy.enemyPath.Waypoints[Random.Range(0, enemy.enemyPath.Waypoints.Count - 1)].position);

            if (enemy.Agent.remainingDistance >= 0.1f)
            {
                stateMachine.ChangeState(new IdleState());
            }
        }

    }
}
