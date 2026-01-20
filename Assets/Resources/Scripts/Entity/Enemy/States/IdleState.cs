using UnityEngine;

public class IdleState : BaseState
{
    private float duration = 0;

    private readonly StateSettings settings;

    public IdleState(IStateContext _stateContext) : base(_stateContext)
    {
        stateContext = _stateContext;
        settings = new StateSettings(stateContext, this, false, false, false, Vector3.zero, 90f);
    }

    protected override void EnterState()
    {
        stateContext.ApplySettings(settings);

        duration = Random.Range(4f, 10f);
    }

    protected override void PerformState()
    {
        if (stateContext.CanSeePlayer())
            stateMachine.ChangeState(new AttackState(stateContext));
        else
        {
            if (stateContext.IsSafe())
            {
                stateMachine.Waiting(new PatrolState(stateContext), duration);
            }
            else
            {
                stateMachine.ChangeState(new FleeState(stateContext));
            }
        }
    }

    protected override void ExitState()
    {
        stateContext.ResetChanges();
    }
}
