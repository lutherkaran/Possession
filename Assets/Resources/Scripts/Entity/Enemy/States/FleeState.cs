using UnityEngine;

public class FleeState : BaseState
{
    Vector3 fleeDirection = Vector3.zero;
    float FleeDistance = 10f;

    public override void Enter()
    {
        enemy.anim.SetBool(Enemy.FLEE, true);
        enemy.Agent.velocity = enemy.defaultVelocity * 4;
    }

    public override void Perform()
    {
        // considering enemy's health is low...

        if (!enemy.IsSafe() || enemy.CanSeePlayer())
        {
            fleeDirection = (enemy.transform.position - enemy.Player.transform.position).normalized + (Random.insideUnitSphere * 5);
            enemy.Agent.SetDestination(enemy.transform.position + fleeDirection * FleeDistance);
        }

        if (enemy.IsSafe() && !enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new HealState());
        }

        else if (enemy.IsSafe() && enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
        }
    }

    public override void Exit()
    {
        enemy.anim.SetBool(Enemy.FLEE, false);
        enemy.Agent.velocity = enemy.defaultVelocity;
    }
}
