using UnityEngine;

public class AttackState : BaseState
{
    private Enemy enemy;

    private float moveTimer;
    private float losePlayerTimer;

    public AttackState(Enemy _enemy) : base(_enemy.gameObject)
    {
        enemy = _enemy;
    }

    protected override void EnterState()
    {
        enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Attacking, true);
        enemy.GetAnimator().AttackingBlend();
    }

    protected override void ExitState()
    {
        enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Attacking, false);
        enemy.GetEnemyAgent().velocity = enemy.defaultVelocity;
 
        moveTimer = 0;
        losePlayerTimer = 0;
    }

    protected override void PerformState()
    {
        if (enemy.CanSeePlayer())
        {
            enemy.transform.LookAt(enemy.GetTargetPlayerTransform());

            enemy.Attack();

            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;

            if (moveTimer > Random.Range(3, 7))
            {
                enemy.GetEnemyAgent().SetDestination(enemy.transform.position + (Random.insideUnitSphere * 5));
                moveTimer = 0;
            }
        }

        else
        {
            losePlayerTimer += Time.deltaTime;

            if (losePlayerTimer > 3)
            {
                stateMachine.ChangeState(new SearchState(enemy));
            }
        }
    }
}
