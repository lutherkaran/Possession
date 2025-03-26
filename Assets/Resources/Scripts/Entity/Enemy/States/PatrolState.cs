using UnityEngine;

public class PatrolState : BaseState
{
    public override void Enter()
    {
        enemy.anim.SetBool(Enemy.IS_PATROLLING, true);
        enemy.Agent.velocity = enemy.defaultVelocity;
        enemy.Agent.SetDestination(enemy.enemyPath.Waypoints[Random.Range(0, enemy.enemyPath.Waypoints.Count - 1)].position);
        enemy.fieldOfView = 150f;
    }

    public override void Perform()
    {
        PatrolCycle();
        if (enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
        }
    }

    public override void Exit()
    {
        enemy.anim.SetBool(Enemy.IS_PATROLLING, false);
    }

    public void PatrolCycle()
    {
        if (enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)//0.2f && (enemy.Agent.remainingDistance >= 0.1f))
        {
            stateMachine.ChangeState(new IdleState());
        }
    }
}
