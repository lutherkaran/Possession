using UnityEngine;

public class PatrolState : BaseState
{
    private new readonly IStateContext stateContext;

    public PatrolState(IStateContext stateContext) : base(stateContext)
    {
        this.stateContext = stateContext;
    }

    protected override void EnterState()
    {
        stateMachine.GetCurrentStateSettings().UpdateSettings(this.stateContext, this, StateSettings.animationStates.isWalking, Vector3.one, 150f);
        stateContext.ApplySettings(stateMachine.GetCurrentStateSettings());
    }

    protected override void PerformState()
    {
        if (stateContext.CanSeePossessedPlayer())
        {
            stateMachine.ChangeState(stateMachine.GetAvailableStates()[typeof(AttackState)]);
            return;
        }

        if (stateContext.CanSeePossessedAnimal())
        {
            stateMachine.ChangeState(stateMachine.GetAvailableStates()[typeof(SuspicionState)]);
            return;
        }

        if (stateContext.IsSafe())
        {
            PatrolCycle();
        }
        else
        {
            stateMachine.ChangeState(stateMachine.GetAvailableStates()[typeof(FleeState)]);
        }
    }

    protected override void ExitState()
    {
        stateContext.ResetChanges();
    }

    protected void PatrolCycle()
    {
        var agent = stateContext.GetNavMeshAgent();

        // An animal that was just possessed/depossessed can momentarily be re-enabled off the
        // baked NavMesh surface (Rigidbody-driven movement during possession isn't constrained
        // to the NavMesh). Reading remainingDistance/stoppingDistance on an agent that isn't
        // actually placed on the mesh throws every frame -- guard it and let it re-settle.
        if (agent == null || !agent.isOnNavMesh) return;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            stateMachine.ChangeState(stateMachine.GetAvailableStates()[typeof(IdleState)]);
        }
    }
}