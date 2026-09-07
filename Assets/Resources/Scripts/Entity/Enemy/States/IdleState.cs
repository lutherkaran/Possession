using UnityEngine;

public class IdleState : BaseState
{
    private float duration = 0;
    private new readonly IStateContext stateContext;

    public IdleState(IStateContext stateContext) : base(stateContext)
    {
        this.stateContext = stateContext;
    }

    protected override void EnterState()
    {
        stateMachine.GetCurrentStateSettings().UpdateSettings(StateSettings.animationStates.isIdle, Vector3.zero, 90f);
        stateContext.ApplySettings(stateMachine.GetCurrentStateSettings());

        duration = Random.Range(4f, 10f);
    }

    protected override void PerformState()
    {
        if (stateContext.CanSeePossessedPlayer())
        {
            stateMachine.ChangeState(stateMachine.GetAvailableStates()[typeof(AttackState)]);
        }
        else if (stateContext.CanSeePossessedAnimal())
        {
            stateMachine.ChangeState(stateMachine.GetAvailableStates()[typeof(SuspicionState)]);
        }
        else
        {
            if (stateContext.IsSafe())
            {
                stateMachine.Waiting(stateMachine.GetAvailableStates()[typeof(PatrolState)], duration);
            }
            else
            {
                stateMachine.ChangeState(stateMachine.GetAvailableStates()[typeof(FleeState)]);
            }
        }
    }

    protected override void ExitState()
    {
        stateContext.ResetChanges();
    }
}