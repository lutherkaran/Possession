using System.Collections;
using UnityEngine;

public class EnemyAI
{
    private Enemy enemy;

    public EnemyAI(Enemy _enemy)
    {
        enemy = _enemy;
    }

    public void RunAI(BaseState a)
    {
        if (a is IdleState)
        {
            enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Idle, true);
            enemy.GetEnemyAgent().velocity = Vector3.zero;
            enemy.GetAnimator().ResetBlend();

            enemy.GetEnemySO().fieldOfView = 90f;
            enemy.GetEnemyAgent().isStopped = true;
        }
        else if (a is PatrolState)
        {
            EnemyManager.instance.enemyPathEnemyDictionary.TryGetValue(enemy, out EnemyPath enemyPath);

            enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Patrolling, true);
            enemy.GetAnimator().WalkBlend();

            enemy.GetEnemyAgent().velocity = enemy.defaultVelocity;
            enemy.GetEnemyAgent().SetDestination(enemyPath.GetRandomPathPosition());
            enemy.GetEnemySO().fieldOfView = 150f;
        }
    }
}
