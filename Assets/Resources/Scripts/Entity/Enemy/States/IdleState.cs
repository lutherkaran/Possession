using UnityEngine;

public class IdleState : BaseState
{
    public float waitTimer = 0;
    public float duration = 0;

    public override void Enter()
    {
        enemy.anim.SetBool(Enemy.IS_IDLE, true);
        enemy.Agent.velocity = Vector3.zero;
        enemy.fieldOfView = 90f;
        duration = Random.Range(4f, 10f);
    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer()) 
        { 
            stateMachine.ChangeState(new AttackState());
        }

        else
        {
            Waiting();
        }
    }

    public override void Exit()
    {
        enemy.anim.SetBool(Enemy.IS_IDLE, false);
        waitTimer = 0;
        enemy.Agent.velocity = enemy.defaultVelocity;
    }

    public void Waiting()
    {
        waitTimer += Time.deltaTime;
        if (waitTimer > duration)
        {
            stateMachine.ChangeState(new PatrolState());
        }
    }
}
