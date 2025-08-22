using UnityEngine;

public class PatrolState : BaseState
{
    private Enemy enemy;

    public PatrolState(Enemy _enemy): base(_enemy.gameObject)
    {
        enemy = _enemy;
    }

    protected override void EnterState()
    {
        enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Patrolling, true);

        enemy.Agent.velocity = enemy.defaultVelocity;
        enemy.Agent.SetDestination(enemy.enemyPath.Waypoints[Random.Range(0, enemy.enemyPath.Waypoints.Count - 1)].position);
        enemy.fieldOfView = 150f;
    }

    protected override void PerformState()
    {
        if (stateMachine.activeState is PossessedState) return;

        PatrolCycle();
        if (enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState(enemy));
        }
    }

    protected override void ExitState()
    {
        enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Patrolling, false);
    }

    protected void PatrolCycle()
    {
        if (enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)//0.2f && (enemy.Agent.remainingDistance >= 0.1f))
        {
            stateMachine.ChangeState(new IdleState(enemy));
        }
    }
}
