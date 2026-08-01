using UnityEngine;

public class PossessedState : BaseState
{
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

    // Movement itself is applied by InputManager.PhysicsRefresh at a fixed timestep now
    // (Rigidbody-driven). This just drives the animator off the same input each frame.
    protected override void PerformState()
    {
        Vector2 moveDir = InputManager.instance.GetMoveDirection();
        float actualSpeed = moveDir.magnitude;
        stateContext.GetAnimationEntity().SetSpeed(actualSpeed);
    }

    protected override void ExitState()
    {
        stateContext.ResetChanges();
    }

}