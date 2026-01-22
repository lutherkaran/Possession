using UnityEngine;

public class PossessedState : BaseState
{
    private readonly float WalkSpeed = 10f;

    private StateSettings stateSettings;

    public PossessedState(IStateContext _stateContext) : base(_stateContext)
    {
        stateContext = _stateContext;

        stateSettings = new StateSettings(stateContext, this, StateSettings.animationStates.isPossessed, Vector3.zero, 0);
    }

    protected override void EnterState()
    {
        base.EnterState();
        stateContext.ApplySettings(stateSettings);
    }

    protected override void PerformState()
    {
        PossessionManager.instance.GetCurrentPossessable().GetPossessedEntity().MoveWhenPossessed(InputManager.instance.GetMoveDirection());
        //    enemy.transform.Translate(moveDirection * WalkSpeed * Time.deltaTime);
        //    enemy.GetAnimator().ManualBlend(WalkSpeed * Time.deltaTime);
    }

    protected override void ExitState()
    {
        stateMachine.ChangeState(stateMachine.lastActiveState);
    }

}
