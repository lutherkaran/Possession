using UnityEngine;

public class PatrolState : BaseState
{
    private Enemy enemy;

    public PatrolState(Enemy _enemy): base(_enemy.gameObject)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Patrolling, true);

        enemy.Agent.velocity = enemy.defaultVelocity;
        enemy.Agent.SetDestination(enemy.enemyPath.Waypoints[Random.Range(0, enemy.enemyPath.Waypoints.Count - 1)].position);
        enemy.fieldOfView = 150f;
    }

    public override void Perform()
    {
        if (stateMachine.activeState is PossessedState) return;

        PatrolCycle();
        if (enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState(enemy));
        }
    }

    public override void Exit()
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
