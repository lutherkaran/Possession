using UnityEngine;

public class SearchState : BaseState
{
    private Enemy enemy;

    private float maxSearchDuration = 20f;
    private float searchTimer;

    private readonly StateSettings stateSettings;

    public SearchState(IStateContext _stateContext) : base(_stateContext)
    {
        stateContext = _stateContext;
        stateSettings = new StateSettings(stateContext, this, false, false, false, Vector3.zero, 180);

        if (stateContext is Enemy enemy)
            this.enemy = enemy;
    }

    protected override void EnterState()
    {
        stateContext.ApplySettings(stateSettings);
    }

    protected override void PerformState()
    {
        searchTimer += Time.deltaTime;

        if (searchTimer < maxSearchDuration)
        {
            if (stateContext.CanSeePlayer())
            {
                stateMachine.ChangeState(new AttackState(stateContext));
            }

            if (stateContext.GetNavMeshAgent().remainingDistance <= stateContext.GetNavMeshAgent().stoppingDistance)
            {
                stateMachine.Waiting(new IdleState(stateContext), 3);
                FindAnotherDestinationNearby();
            }
        }

        else
        {
            stateMachine.ChangeState(new PatrolState(stateContext));
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
        stateMachine.ChangeState(new SearchState(stateContext));
        // It should be player's last position not the current possition.
        stateContext.GetNavMeshAgent().SetDestination(stateContext.GetTransform().position + PlayerManager.instance.GetPlayer().transform.position + (Random.insideUnitSphere * 20f));
    }
}
