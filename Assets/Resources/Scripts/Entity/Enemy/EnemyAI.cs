public class EnemyAI
{
    private Enemy enemy;
    private StateSettings stateSettings;

    public EnemyAI(Enemy _enemy)
    {
        enemy = _enemy;
    }

    public void RunAI(StateSettings _stateSettings)
    {
        stateSettings = _stateSettings;

        if (stateSettings.currentActiveState is IdleState)
        {
            enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Idle, stateSettings.isIdle);
            enemy.GetEnemyAgent().velocity = stateSettings.desiredVelocity;
            enemy.GetAnimator().ResetBlend();

            enemy.GetEnemySO().fieldOfView = stateSettings.fieldOfView;
            enemy.GetEnemyAgent().isStopped = stateSettings.isIdle;
        }
        else if (stateSettings.currentActiveState is PatrolState)
        {
            EnemyManager.instance.enemyPathEnemyDictionary.TryGetValue(enemy, out EnemyPath enemyPath);

            enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Patrolling, stateSettings.isPatrolling);
            enemy.GetEnemyAgent().isStopped = false;
            enemy.GetAnimator().WalkBlend();

            enemy.GetEnemyAgent().velocity = enemy.defaultVelocity;
            enemy.GetEnemyAgent().SetDestination(enemyPath.GetRandomPathPosition());
            enemy.GetEnemySO().fieldOfView = stateSettings.fieldOfView;
        }
        else if (stateSettings.currentActiveState is AttackState)
        {
            enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Attacking, true);
            enemy.GetAnimator().AttackingBlend();
        }
        else if (stateSettings.currentActiveState is SearchState)
        {
            enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Searching, true);
            enemy.GetAnimator().RunBlend();
            enemy.GetEnemyAgent().SetDestination(enemy.targetsLastPosition);
            enemy.GetEnemyAgent().velocity = enemy.defaultVelocity * 4f;
            enemy.GetEnemySO().fieldOfView = stateSettings.fieldOfView;
        }
    }

    public void Reset()
    {
        if (stateSettings.currentActiveState is IdleState)
        {
            enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Idle, false);
        }
        else if (stateSettings.currentActiveState is PatrolState)
        {
            enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Patrolling, false);
        }
        else if (stateSettings.currentActiveState is AttackState)
        {
            enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Attacking, false);
            enemy.GetEnemyAgent().velocity = enemy.defaultVelocity;
        }
        else if (stateSettings.currentActiveState is SearchState)
        {
            enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Searching, false);
            enemy.GetEnemyAgent().velocity = enemy.defaultVelocity;
        }
    }
}

