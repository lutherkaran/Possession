using UnityEngine;

public class PatrolState : BaseState
{
    private Enemy enemy;
    private EnemyPath enemyPath;

    public PatrolState(Enemy _enemy) : base(_enemy.gameObject)
    {
        enemy = _enemy;
    }

    protected override void EnterState()
    {
        EnemyManager.instance.enemyPathEnemyDictionary.TryGetValue(enemy, out EnemyPath enemyPath);

        enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Patrolling, true);
        enemy.GetAnimator().WalkBlend();

        enemy.GetEnemyAgent().velocity = enemy.defaultVelocity;
        enemy.GetEnemyAgent().SetDestination(enemyPath.GetRandomPathPosition());
        enemy.GetEnemySO().fieldOfView = 150f;
    }

    protected override void PerformState()
    {
        PatrolCycle();

        if (enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState(enemy));
        }
    }

    protected override void ExitState()
    {
        enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Patrolling, false);
    }

    protected void PatrolCycle()
    {
        if (enemy.GetEnemyAgent().remainingDistance <= enemy.GetEnemyAgent().stoppingDistance)//0.2f && (enemy.Agent.remainingDistance >= 0.1f))
        {
            stateMachine.ChangeState(new IdleState(enemy));
        }
    }
}
