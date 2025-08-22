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

    public override void Enter()
    {
        enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Running, true);

        enemy.Agent.velocity = enemy.defaultVelocity * 4f;
    }

    public override void Perform()
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

    public override void Exit()
    {
        enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Running, false);
        enemy.Agent.velocity = enemy.defaultVelocity;
    }
}
