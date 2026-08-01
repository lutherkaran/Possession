using UnityEngine;
using UnityEngine.AI;

public class FleeState : BaseState
{
    private readonly StateSettings settings;

    private Vector3 lastFleeTarget;
    private float fleeDistance = 4f;

    public FleeState(IStateContext _stateContext) : base(_stateContext)
    {
        stateContext = _stateContext;

        settings = new StateSettings(stateContext, this, StateSettings.animationStates.isRunning, Vector3.zero, 180f);
    }

    protected override void EnterState()
    {
        stateContext.ApplySettings(settings);
    }

    protected override void PerformState()
    {
        if (stateContext.IsSafe())
        {
            stateMachine.ChangeState(stateMachine.lastActiveState ?? new IdleState(stateContext));
        }
        else
            Flee();
    }

    protected override void ExitState()
    {
        stateContext.ResetChanges();
    }

    private void Flee()
    {
        Vector3 fleeDir = CalculateFleeDirection();
        if (fleeDir == Vector3.zero) fleeDir = stateContext.GetTransform().forward;

        Vector3 randomOffset = new Vector3(
            Random.Range(-8f, 8f),
            0f,                      // no random Y
            Random.Range(-8f, 8f)
        );

        Vector3 targetPos = stateContext.GetTransform().position + fleeDir * fleeDistance + randomOffset;

        if (NavMesh.SamplePosition(targetPos, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            targetPos = hit.position;
        }

        stateContext.GetNavMeshAgent().SetDestination(targetPos);
        lastFleeTarget = targetPos;
    }

    // Was keyed to PlayerManager.GetPlayer() (the abandoned human body's fixed position) even
    // when the actual threat is whatever is currently possessed -- an animal fleeing capture
    // should run from the guard/possessed threat, not from wherever the player's body is parked.
    private Vector3 CalculateFleeDirection()
    {
        Transform threat = PossessionManager.instance.GetActiveTransform();
        if (threat == null) return stateContext.GetTransform().forward;
        return (stateContext.GetTransform().position - threat.position).normalized;
    }
}