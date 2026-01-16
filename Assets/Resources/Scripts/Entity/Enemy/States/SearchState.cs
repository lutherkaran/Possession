using System.Collections;
using System.Threading;
using UnityEngine;

public class SearchState : BaseState
{
    private Enemy enemy;

    private float maxSearchDuration = 20f;
    private float searchTimer;

    public SearchState(Enemy _enemy) : base(_enemy.gameObject)
    {
        enemy = _enemy;
    }

    protected override void EnterState()
    {
        enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Searching, true);
        enemy.GetAnimator().RunBlend();
        enemy.GetEnemyAgent().SetDestination(enemy.targetsLastPosition);
        enemy.GetEnemyAgent().velocity = enemy.defaultVelocity * 4f;
        enemy.GetEnemySO().fieldOfView = 180f;
    }

    protected override void PerformState()
    {
        searchTimer += Time.deltaTime;

        if (searchTimer < maxSearchDuration)
        {
            if (stateContext.CanSeePlayer())
            {
                stateMachine.ChangeState(new AttackState(enemy));
            }

            if (enemy.GetEnemyAgent().remainingDistance <= enemy.GetEnemyAgent().stoppingDistance)
            {
                stateMachine.Waiting(new IdleState(enemy), 3);
                FindAnotherDestinationNearby();
            }
        }

        else
        {
            stateMachine.ChangeState(new PatrolState(enemy));
        }
    }

    protected override void ExitState()
    {
        enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Searching, false);
        enemy.StopAllCoroutines();
        searchTimer = 0;
    }

    private void FindAnotherDestinationNearby()
    {
        stateMachine.ChangeState(new SearchState(enemy));
        enemy.GetEnemyAgent().SetDestination(enemy.targetsLastPosition + (Random.insideUnitSphere * 20f));
    }
}
