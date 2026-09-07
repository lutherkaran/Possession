using UnityEngine;

public class PatrolState : BaseState
{
    private new readonly IStateContext stateContext;

    public PatrolState(IStateContext stateContext) : base(stateContext)
    {
        this.stateContext = stateContext;
        stateType = StateType.Patrol;
    }

    protected override void EnterState()
    {
        stateMachine.GetCurrentStateSettings().UpdateSettings(StateSettings.animationStates.isWalking, Vector3.one, 150f);
        stateContext.ApplySettings(stateMachine.GetCurrentStateSettings());
    }

    protected override void PerformState()
    {
        if (stateContext.CanSeePossessedPlayer())
        {
            stateMachine.ChangeState(stateMachine.GetAvailableStates()[BaseState.StateType.Attack]);
            return;
        }

        if (stateContext.CanSeePossessedAnimal())
        {
            stateMachine.ChangeState(stateMachine.GetAvailableStates()[BaseState.StateType.Suspicion]);
            return;
        }

        if (stateContext.IsSafe())
        {
            PatrolCycle();
        }
        else
        {
            stateMachine.ChangeState(stateMachine.GetAvailableStates()[BaseState.StateType.Flee]);
        }
    }

    protected override void ExitState()
    {
        stateContext.ResetChanges();
    }

    protected void PatrolCycle()
    {
        var agent = stateContext.GetNavMeshAgent();

        if (agent == null || !agent.isOnNavMesh) return;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            stateMachine.ChangeState(stateMachine.GetAvailableStates()[BaseState.StateType.Idle]);
        }
    }
}