using UnityEngine;

public class FleeState : BaseState
{
    Vector3 fleeDirection = Vector3.zero;

    public override void Enter()
    {
        enemy.Agent.speed = 4;
        enemy.anim.SetBool(Enemy.FLEE, true);
    }

    public override void Perform()
    {
        if (enemy.IsSafe() & !enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new HealState());
        }
        else
        {
            fleeDirection = (enemy.transform.position - enemy.Player.transform.position).normalized + (Random.insideUnitSphere * 5);
            enemy.Agent.SetDestination(enemy.transform.position + fleeDirection * 10f);
        }
    }

    public override void Exit()
    {
        enemy.anim.SetBool(Enemy.FLEE, false);
    }
}
