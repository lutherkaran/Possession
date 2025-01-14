using UnityEngine;

public class IdleState : BaseState
{
    public float waitTimer = 0;
    public float duration = 0;

    public override void Enter()
    {
        enemy.anim.Play(Enemy.IDLE);
        enemy.anim.SetBool(Enemy.PATROLLING, false);
        enemy.anim.SetBool(Enemy.ATTACK, false);
        duration = Random.Range(4, 10);
        enemy.Agent.speed = 0;
    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer()) { stateMachine.ChangeState(new AttackState()); }
        else
        {
            Waiting();
        }
    }

    public override void Exit()
    {
        waitTimer = 0;
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
