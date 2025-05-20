using UnityEngine;

public class FleeState : BaseState
{
    Vector3 fleeDirection = Vector3.zero;
    float FleeDistance = 10f;

    public override void Enter()
    {
        enemy.GetAnimator().SetBool(Enemy.IS_FLEEING, true);
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
            stateMachine.ChangeState(new HealState());
        }
    }

    private void Flee()
    {
        fleeDirection = (enemy.transform.position - enemy.player.transform.position).normalized + (Random.insideUnitSphere * 10).normalized;
        enemy.Agent.SetDestination(enemy.transform.position + fleeDirection * FleeDistance);
    }

    public override void Exit()
    {
        enemy.GetAnimator().SetBool(Enemy.IS_FLEEING, false);
        enemy.Agent.velocity = enemy.defaultVelocity;
    }
}
