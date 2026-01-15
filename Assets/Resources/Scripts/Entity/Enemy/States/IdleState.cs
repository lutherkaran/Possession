using UnityEngine;

public class IdleState : BaseState
{
    private NPCAI npcAi;
    private Enemy enemy;
    private Npc npc;
    private AnimalNpc animalNpc;

    private EnemyAnimator enemyAnimator;

    private float duration = 0;

    public IdleState(NPCAI npcAi) : base(npcAi.gameObject)
    {
        this.npcAi = npcAi;

        if (npcAi is Enemy e)
            enemy = e;
        else if (npcAi is Npc n)
            npc = n;
        else if (npcAi is AnimalNpc a)
            animalNpc = a;
    }

    protected override void EnterState()
    {
        npcAi.MakeChanges(this);

        duration = Random.Range(4f, 10f);
    }

    protected override void PerformState()
    {
        //if (enemy)
        //{
        //    if (enemy.CanSeePlayer())
        //    {
        //        stateMachine.ChangeState(new AttackState(enemy));
        //    }
        //    else
        //    {
        //        stateMachine.Waiting(new PatrolState(enemy), duration);
        //    }
        //}

        if (animalNpc)
        {
            stateMachine.Waiting(new PatrolState(animalNpc), duration);
        }
    }

    protected override void ExitState()
    {
        if (enemy)
        {
            enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Idle, false);
            enemy.GetEnemyAgent().velocity = enemy.defaultVelocity;
            enemy.GetEnemyAgent().isStopped = false;
        }

        if (animalNpc)
        {
            animalNpc.GetAnimal().GetAnimalAnimator().SetBool("IsWalking", true);
            animalNpc.GetAnimal().GetAnimalNpcAgent().isStopped = false;
            animalNpc.GetAnimal().GetAnimalNpcAgent().velocity = Vector3.one;
        }
    }
}
