using UnityEngine;

public class PatrolState : BaseState
{
    private readonly StateSettings settings;

    public PatrolState(IStateContext _stateContext) : base(_stateContext)
    {
        stateContext = _stateContext;

        settings = new StateSettings(stateContext, this, false, false, true, Vector3.zero, 150f);
    }

    protected override void EnterState()
    {
        stateContext.ApplySettings(settings);
    }

    protected override void PerformState()
    {
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
