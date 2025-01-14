using UnityEngine;

public class PatrolState : BaseState
{
    public int waypointIndex = 0;
    public float waitTimer;

    public override void Enter()
    {
        enemy.anim.SetBool(Enemy.PATROLLING, true);
        enemy.anim.SetBool(Enemy.ATTACK, false);
        enemy.anim.SetBool(Enemy.FLEE, false);
        enemy.Agent.speed = 2;
    }

    public override void Perform()
    {
        PatrolCycle();

        if (enemy.CanSeePlayer())
        {
            if(enemy.GetHealth() < 30) 
            {
                stateMachine.ChangeState(new FleeState());
            }

            stateMachine.ChangeState(new AttackState());
        }
    }

    public override void Exit()
    {
        waitTimer = 0;
        waypointIndex = 0;
        enemy.anim.SetBool(Enemy.PATROLLING, false);
    }

    public void PatrolCycle()
    {
        if (enemy.Agent.remainingDistance < 0.2f)
        {
            if (enemy.Agent.remainingDistance >= 0.1f)
            {
                stateMachine.ChangeState(new IdleState());
            }
            enemy.Agent.SetDestination(enemy.enemyPath.Waypoints[Random.Range(0, enemy.enemyPath.Waypoints.Count - 1)].position);
        }

    }
}
