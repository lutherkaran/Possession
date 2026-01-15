using UnityEngine;

public class PatrolState : BaseState
{
    private NPCAI nPCAI;
    private Enemy enemy;
    private Npc npc;
    private AnimalNpc animalNpc;

    private EnemyPath enemyPath;

    public PatrolState (NPCAI nPCAI) : base(nPCAI.gameObject)
    {
        this.nPCAI = nPCAI;

        if (nPCAI is Enemy e)
            enemy = e;
        else if (nPCAI is Npc n)
            npc = n;
        else if (nPCAI is AnimalNpc a)
            animalNpc = a;
    }

    protected override void EnterState()
    {
        nPCAI.MakeChanges(this);

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

        if (enemy)
        {
            if (enemy.CanSeePlayer())
            {
                stateMachine.ChangeState(new AttackState(enemy));
            }
        }
    }

    protected override void ExitState()
    {
        enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Patrolling, false);
    }

    protected void PatrolCycle()
    {
        if (enemy)
        {
            if (enemy.GetEnemyAgent().remainingDistance <= enemy.GetEnemyAgent().stoppingDistance)//0.2f && (enemy.Agent.remainingDistance >= 0.1f))
            {
                stateMachine.ChangeState(new IdleState(enemy));
            }
        }
        if (animalNpc)
        {
            if (animalNpc.GetAnimalNpcAgent().remainingDistance <= animalNpc.GetAnimalNpcAgent().stoppingDistance)
            {
                stateMachine.ChangeState(new IdleState(animalNpc));
            }
        }
    }
}
