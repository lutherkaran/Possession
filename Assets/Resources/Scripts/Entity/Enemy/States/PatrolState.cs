using UnityEngine;

public class PatrolState : BaseState
{

    private Enemy enemy;
    private Npc npc;
    private AnimalNpc animalNpc;

    private EnemyPath enemyPath;

    private readonly StateSettings settings;

    public PatrolState(IStateContext _stateContext) : base(_stateContext)
    {
        stateContext = _stateContext;

        settings = new StateSettings(stateContext, this, false, false, Vector3.zero, Vector3.zero, 150f);
    }

    protected override void EnterState()
    {
        stateContext.MakeChanges(settings);

        if (enemy)
        {
            //EnemyManager.instance.enemyPathEnemyDictionary.TryGetValue(enemy, out EnemyPath enemyPath);
            //
            //enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Patrolling, true);
            //enemy.GetAnimator().WalkBlend();
            //
            //enemy.GetEnemyAgent().velocity = enemy.defaultVelocity;
            //enemy.GetEnemyAgent().SetDestination(enemyPath.GetRandomPathPosition());
            //enemy.GetEnemySO().fieldOfView = 150f;
        }

        if (animalNpc)
        {
            //animalNpc.GetAnimal().GetComponent<Chicken>().MoveToLocation(Time.fixedDeltaTime);
            //animalNpc.GetAnimal().GetAnimalAnimator().SetBool("IsWalking", true);
        }
    }

    protected override void PerformState()
    {
        PatrolCycle();

        if (stateContext.CanSeePlayer())
            stateMachine.ChangeState(new AttackState(stateContext));
    }

    protected override void ExitState()
    {
        stateContext.ResetChanges();
        //enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Patrolling, false);
    }

    protected void PatrolCycle()
    {
        if (stateContext.GetNavMeshAgent().remainingDistance <= stateContext.GetNavMeshAgent().stoppingDistance)
        {
            stateMachine.ChangeState(new IdleState(stateContext));
        }
    }
}
