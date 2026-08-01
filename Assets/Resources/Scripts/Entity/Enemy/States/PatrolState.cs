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
        // Previously a patrolling guard never checked for either threat until it happened to
        // stop and re-enter IdleState -- meaning a guard actively walking its route was
        // effectively blind. Both checks now run every frame here too.
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
        if (stateContext.GetNavMeshAgent().remainingDistance <= stateContext.GetNavMeshAgent().stoppingDistance)
        {
            stateMachine.ChangeState(new IdleState(stateContext));
        }
    }
}