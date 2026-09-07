using UnityEngine;
using UnityEngine.AI;

public class FleeState : BaseState
{
    private readonly float fleeDistance = 4f;
    private new readonly IStateContext stateContext;

    public FleeState(IStateContext stateContext) : base(stateContext)
    {
        this.stateContext = stateContext;
    }

    protected override void EnterState()
    {
        stateMachine.GetCurrentStateSettings().UpdateSettings(StateSettings.animationStates.isRunning, Vector3.zero, 180f);
        stateContext.ApplySettings(stateMachine.GetCurrentStateSettings());
    }

    protected override void PerformState()
    {
        if (stateContext.IsSafe())
        {
            stateMachine.ChangeState(stateMachine.lastActiveState ?? stateMachine.GetAvailableStates()[typeof(IdleState)]);
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

        Vector3 randomOffset = new Vector3(Random.Range(-8f, 8f), 0f, Random.Range(-8f, 8f));
        Vector3 targetPos = stateContext.GetTransform().position + fleeDir * fleeDistance + randomOffset;

        if (NavMesh.SamplePosition(targetPos, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            targetPos = hit.position;
        }

        stateContext.GetNavMeshAgent().SetDestination(targetPos);
    }

    private Vector3 CalculateFleeDirection()
    {
        Transform threat = PossessionManager.instance.GetActiveTransform();
        if (threat == null) return stateContext.GetTransform().forward;
        return (stateContext.GetTransform().position - threat.position).normalized;
    }
}