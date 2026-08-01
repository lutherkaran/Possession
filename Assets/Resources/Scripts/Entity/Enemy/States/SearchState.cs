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
        stateSettings = new StateSettings(stateContext, this, StateSettings.animationStates.isRunning, Vector3.zero, 180);

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
            if (stateContext.CanSeePossessedPlayer())
            {
                stateMachine.ChangeState(new AttackState(stateContext));
                return;
            }

            if (stateContext.GetNavMeshAgent().remainingDistance <= stateContext.GetNavMeshAgent().stoppingDistance)
            {
                FindAnotherDestinationNearby();
                stateMachine.Waiting(new IdleState(stateContext), 3);
            }
        }
        else
        {
            stateMachine.ChangeState(new PatrolState(stateContext));
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

    // Was previously summing the guard's own position with the player's world position
    // (nonsensical -- could point anywhere off the map). Now searches near the actual last
    // known sighting instead.
    private void FindAnotherDestinationNearby()
    {
        Vector3 lastKnown = enemy != null ? enemy.targetsLastPosition : stateContext.GetTransform().position;
        stateContext.GetNavMeshAgent().SetDestination(lastKnown + (Random.insideUnitSphere * 10f));
    }
}