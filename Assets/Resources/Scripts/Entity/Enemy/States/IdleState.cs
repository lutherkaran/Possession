using UnityEngine;

public class IdleState : BaseState
{
    private Enemy enemy;
    private AnimalNpc animalNpc;

    private EnemyAnimator enemyAnimator;

    private float duration = 0;

    public IdleState(Entity entity) : base(entity.gameObject)
    {
        if (entity is Enemy e)
            enemy = e;
        else if (entity is AnimalNpc a)
            animalNpc = a;
    }

    protected override void EnterState()
    {
        duration = Random.Range(4f, 10f);

        if (enemy)
        {
            enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Idle, true);
            enemy.GetEnemyAgent().velocity = Vector3.zero;
            enemy.GetAnimator().ResetBlend();

            enemy.GetEnemySO().fieldOfView = 90f;
            enemy.GetEnemyAgent().isStopped = true;
        }

        if (animalNpc)
        {
            animalNpc.GetAnimal().GetAnimalAnimator().SetBool("IsWalking", false); // idle true
            animalNpc.GetAnimalNpcAgent().velocity = Vector3.zero;

            animalNpc.GetAnimal().GetAnimalNpcAgent().isStopped = true;
        }
    }

    protected override void PerformState()
    {
        if (enemy)
        {
            if (enemy.CanSeePlayer())
            {
                stateMachine.ChangeState(new AttackState(enemy));
            }
            else
            {
                stateMachine.Waiting(new PatrolState(enemy), duration);
            }
        }

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

        if(animalNpc)
        {
            animalNpc.GetAnimal().GetAnimalAnimator().SetBool("IsWalking", true);
            animalNpc.GetAnimal().GetAnimalNpcAgent().isStopped = false;
            animalNpc.GetAnimal().GetAnimalNpcAgent().velocity = Vector3.one;
        }
    }
}
