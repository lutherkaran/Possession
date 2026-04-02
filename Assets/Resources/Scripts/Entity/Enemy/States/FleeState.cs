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
            stateMachine.ChangeState(stateMachine.lastActiveState);
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

    private Vector3 CalculateFleeDirection()
    {
        return ((stateContext.GetTransform().position - PlayerManager.instance.GetPlayer().transform.position)).normalized;
    }
}
