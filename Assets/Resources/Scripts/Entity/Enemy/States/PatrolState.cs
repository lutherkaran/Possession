using UnityEngine;

public class PatrolState : BaseState
{
    private readonly StateSettings settings;

    public PatrolState(IStateContext _stateContext) : base(_stateContext)
    {
        stateContext = _stateContext;

        settings = new StateSettings(stateContext, this, StateSettings.animationStates.isWalking, Vector3.one, 150f);
    }

    protected override void EnterState()
    {
        stateContext.ApplySettings(settings);
    }

    protected override void PerformState()
    {
        if (stateContext.CanSeePossessedPlayer())
        {
            stateMachine.ChangeState(new AttackState(stateContext));
            return;
        }

        if (stateContext.CanSeePossessedAnimal())
        {
            stateMachine.ChangeState(new SuspicionState(stateContext));
            return;
        }

        if (stateContext.IsSafe())
        {
            PatrolCycle();
        }
        else
        {
            stateMachine.ChangeState(new FleeState(stateContext));
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
            stateMachine.ChangeState(new IdleState(stateContext));
        }
    }
}