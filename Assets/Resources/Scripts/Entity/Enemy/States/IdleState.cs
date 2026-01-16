using UnityEngine;

public class IdleState : BaseState
{
    private float duration = 0;

    private readonly StateSettings settings;

    public IdleState(IStateContext _stateContext) : base(_stateContext)
    {
        stateContext = _stateContext;
        settings = new StateSettings(stateContext, this, false, false, Vector3.zero, Vector3.zero, 90f);
    }

    protected override void EnterState()
    {
        stateContext.MakeChanges(settings);

        duration = Random.Range(4f, 10f);
    }

    protected override void PerformState()
    {
        if (stateContext.CanSeePlayer())
            stateMachine.ChangeState(new AttackState(stateContext));
        else
            stateMachine.Waiting(new PatrolState(stateContext), duration);
    }

    protected override void ExitState()
    {
        stateContext.ResetChanges();

        //if (enemy)
        //{
        //    enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Idle, false);
        //    enemy.GetEnemyAgent().velocity = enemy.defaultVelocity;
        //    enemy.GetEnemyAgent().isStopped = false;
        //}

        //if (animalNpc)
        //{
        //    animalNpc.GetAnimal().GetAnimalAnimator().SetBool("IsWalking", true);
        //    animalNpc.GetAnimal().GetAnimalNpcAgent().velocity = Vector3.one;
        //    animalNpc.GetAnimal().GetAnimalNpcAgent().isStopped = false;
        //}
    }
}
