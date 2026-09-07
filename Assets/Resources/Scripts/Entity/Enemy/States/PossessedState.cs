using UnityEngine;

public class PossessedState : BaseState
{
    private new readonly IStateContext stateContext;

    public PossessedState(IStateContext stateContext) : base(stateContext)
    {
        this.stateContext = stateContext;
    }

    protected override void EnterState()
    {
        base.EnterState();
        stateMachine.GetCurrentStateSettings().UpdateSettings(StateSettings.animationStates.isPossessed, Vector3.zero, 0);
        stateContext.ApplySettings(stateMachine.GetCurrentStateSettings());
    }

    protected override void PerformState()
    {
        Vector2 moveDir = InputManager.instance.GetMoveDirection();
        var entity = PossessionManager.instance.GetCurrentPossessable().GetPossessedEntity();

        float blend = moveDir.magnitude * (entity.IsSprinting() ? 2f : 1f);
        stateContext.GetAnimationEntity().SetSpeed(blend);
    }

    protected override void ExitState()
    {
        stateContext.ResetChanges();
    }
}