using UnityEngine;

public class IdleState : BaseState
{
    private Enemy enemy;
    private EnemyAnimator enemyAnimator;

    private float duration = 0;

    public IdleState(Enemy _enemy) : base(_enemy.gameObject)
    {
        enemy = _enemy;
    }

    protected override void EnterState()
    {
        enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Idle, true);
        enemy.GetEnemyAgent().velocity = Vector3.zero;
        enemy.GetAnimator().ResetBlend();

        enemy.fieldOfView = 90f;
        duration = Random.Range(4f, 10f);
        enemy.GetEnemyAgent().isStopped = false;
    }

    protected override void PerformState()
    {
        if (enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState(enemy));
        }
        else
        {
            stateMachine.Waiting(new PatrolState(enemy), duration);
        }
    }

    protected override void ExitState()
    {
        enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Idle, false);
        enemy.GetEnemyAgent().velocity = enemy.defaultVelocity;
    }
}
