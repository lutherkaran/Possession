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
        duration = Random.Range(1, 6);
        enemy.Agent.speed = 0;
    }

    public override void Perform()
    {
        Waiting();
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
