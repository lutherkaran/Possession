using UnityEngine;

public class IdleState : BaseState
{
    private float duration = 0;
    private new readonly IStateContext stateContext;

    public IdleState(IStateContext stateContext) : base(stateContext)
    {
        this.stateContext = stateContext;
        stateType = StateType.Idle;
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
            stateMachine.ChangeState(stateMachine.GetAvailableStates()[BaseState.StateType.Attack]);
        }
        else if (stateContext.CanSeePossessedAnimal())
        {
            stateMachine.ChangeState(stateMachine.GetAvailableStates()[BaseState.StateType.Suspicion]);
        }
        else
        {
            if (stateContext.IsSafe())
            {
                stateMachine.Waiting(stateMachine.GetAvailableStates()[BaseState.StateType.Patrol], duration);
            }
            else
            {
                stateMachine.ChangeState(stateMachine.GetAvailableStates()[BaseState.StateType.Flee]);
            }
        }
    }

    protected override void ExitState()
    {
        stateContext.ResetChanges();
    }
}