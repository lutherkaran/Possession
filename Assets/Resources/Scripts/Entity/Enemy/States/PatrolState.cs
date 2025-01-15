using UnityEngine;

public class PatrolState : BaseState
{
    public int waypointIndex = 0;
    public float waitTimer;

    public override void Enter()
    {
        enemy.anim.SetBool(Enemy.PATROLLING, true);
        enemy.Agent.velocity = enemy.defaultVelocity;
        enemy.Agent.SetDestination(enemy.enemyPath.Waypoints[Random.Range(0, enemy.enemyPath.Waypoints.Count - 1)].position);
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
        enemy.anim.SetBool(Enemy.PATROLLING, false);
        waitTimer = 0;
        waypointIndex = 0;
    }

    public void PatrolCycle()
    {
        if (enemy.Agent.remainingDistance < 0.2f)
        {
            if (enemy.Agent.remainingDistance >= 0.1f)
            {
                stateMachine.ChangeState(new IdleState());
            }
      
        }

    }
}
