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

    // Movement itself is applied by InputManager.PhysicsRefresh at a fixed timestep (Rigidbody-
    // driven). This drives the animator off the same input each frame.
    //
    // The Player's blend tree has THREE poses on a 0..2 range (Idle=0, Walk=1, Run=2), but this
    // was only ever sending moveDir.magnitude (0..1) -- meaning the "Run" pose was mathematically
    // unreachable regardless of actual speed. Sprinting moved the Rigidbody ~1.7x faster while the
    // animation stayed capped at the Walk pose the whole time -- that mismatch between visual
    // walk-pace and actual travel speed is what read as "sliding"/"skating". Scaling by 2 while
    // sprinting lets the blend value actually reach the Run pose. (Entities whose blend tree tops
    // out at 1, like the animals, just clamp gracefully at their own top pose -- harmless.)
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