using UnityEngine;

public class FleeState : BaseState
{
    private Enemy enemy;

    private Vector3 fleeDirection = Vector3.zero;
    private float FleeDistance = 10f;

    public FleeState(Enemy _enemy) : base(_enemy.gameObject)
    {
        enemy = _enemy;
    }

    protected override void EnterState()
    {
        enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Running, true);

        enemy.GetEnemyAgent().velocity = enemy.defaultVelocity * 4f;
    }

    protected override void PerformState()
    {
        if (!enemy.IsSafe() && enemy.CanSeePlayer())
        {
            Flee();
        }
        else
        {
            stateMachine.ChangeState(new HealState(enemy));
        }
    }

    protected override void ExitState()
    {
        enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Running, false);
        enemy.GetEnemyAgent().velocity = enemy.defaultVelocity;
    }

    private void Flee()
    {
        fleeDirection = (enemy.transform.position - enemy.GetTargetPlayerTransform().position).normalized + (Random.insideUnitSphere * 10).normalized;
        enemy.GetEnemyAgent().SetDestination(enemy.transform.position + fleeDirection * FleeDistance);
    }
}
