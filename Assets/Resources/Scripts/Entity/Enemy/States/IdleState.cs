using UnityEngine;

public class IdleState : BaseState
{
    private Enemy enemy;
    private EnemyAnimator enemyAnimator;

    public IdleState(Enemy _enemy) : base(_enemy.gameObject)
    {
        enemy = _enemy;
    }

    public float waitTimer = 0;
    public float duration = 0;

    protected override void EnterState()
    {
        enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Idle, true);
        enemy.Agent.velocity = Vector3.zero;
        enemy.fieldOfView = 90f;
        duration = Random.Range(4f, 10f);
        enemy.Agent.isStopped = false;
    }

    protected override void PerformState()
    {
        if (enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState(enemy));
        }
        else
        {
            Waiting();
        }
    }

    protected override void ExitState()
    {
        enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Idle, false);
        waitTimer = 0;
        enemy.Agent.velocity = enemy.defaultVelocity;
    }

    public void Waiting()
    {
        if (!enemy.CanSeePlayer())
        {
            waitTimer += Time.deltaTime;
            if (waitTimer > duration)
            {
                stateMachine.ChangeState(new PatrolState(enemy));
            }
        }
    }
}
