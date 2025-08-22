using UnityEngine;

public class FleeState : BaseState
{
    private Enemy enemy;

    public FleeState(Enemy _enemy) : base(_enemy.gameObject)
    {
        enemy = _enemy;
    }

    Vector3 fleeDirection = Vector3.zero;
    float FleeDistance = 10f;

    protected override void EnterState()
    {
        enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Running, true);

        enemy.Agent.velocity = enemy.defaultVelocity * 4f;
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

    public void Flee()
    {
        fleeDirection = (enemy.transform.position - enemy.player.transform.position).normalized + (Random.insideUnitSphere * 10).normalized;
        enemy.Agent.SetDestination(enemy.transform.position + fleeDirection * FleeDistance);
    }

    protected override void ExitState()
    {
        enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Running, false);
        enemy.Agent.velocity = enemy.defaultVelocity;
    }
}
