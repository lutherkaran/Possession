using UnityEngine;

public class SearchState : BaseState
{
    private readonly Enemy enemy;
    private new readonly IStateContext stateContext;

    private float maxSearchDuration = 20f;
    private float searchTimer;

    public SearchState(IStateContext stateContext) : base(stateContext)
    {
        this.stateContext = stateContext;
        if (this.stateContext is Enemy enemy)
            this.enemy = enemy;
    }

    protected override void EnterState()
    {
        stateMachine.GetCurrentStateSettings().UpdateSettings(StateSettings.animationStates.isRunning, Vector3.zero, 180f);
        stateContext.ApplySettings(stateMachine.GetCurrentStateSettings());
    }

    protected override void PerformState()
    {
        searchTimer += Time.deltaTime;

        if (searchTimer < maxSearchDuration)
        {
            if (stateContext.CanSeePossessedPlayer())
            {
                stateMachine.ChangeState(stateMachine.GetAvailableStates()[typeof(AttackState)]);
                return;
            }

            if (stateContext.GetNavMeshAgent().remainingDistance <= stateContext.GetNavMeshAgent().stoppingDistance)
            {
                FindAnotherDestinationNearby();
                stateMachine.Waiting(stateMachine.GetAvailableStates()[typeof(IdleState)], 3);
            }
        }
        else
        {
            stateMachine.ChangeState(stateMachine.GetAvailableStates()[typeof(PatrolState)]);
        }
    }

    protected override void ExitState()
    {
        if (enemy != null)
        {
            enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Searching, false);
            enemy.StopAllCoroutines();
        }
        searchTimer = 0;
    }

    private void FindAnotherDestinationNearby()
    {
        Vector3 lastKnown = enemy != null ? enemy.targetsLastPosition : stateContext.GetTransform().position;
        stateContext.GetNavMeshAgent().SetDestination(lastKnown + (Random.insideUnitSphere * 10f));
    }
}