public class EnemyController
{
    private Enemy enemy;
    private StateSettings stateSettings;

    public EnemyController(Enemy _enemy)
    {
        enemy = _enemy;
    }

    public void RunAI(StateSettings _stateSettings)
    {
        stateSettings = _stateSettings;

        if (stateSettings.currentActiveState is IdleState)
        {
            enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Idle, stateSettings.animStates == StateSettings.animationStates.isIdle);
            enemy.GetEnemyAgent().velocity = stateSettings.desiredVelocity;
            enemy.GetAnimator().ResetBlend();

            enemy.GetEnemySO().fieldOfView = stateSettings.fieldOfView;
            enemy.GetEnemyAgent().isStopped = stateSettings.animStates == StateSettings.animationStates.isIdle;
        }
        else if (stateSettings.currentActiveState is PatrolState)
        {
            EnemyManager.instance.enemyPathEnemyDictionary.TryGetValue(enemy, out EnemyPath enemyPath);

            enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Patrolling, stateSettings.animStates == StateSettings.animationStates.isWalking);
            enemy.GetEnemyAgent().isStopped = stateSettings.animStates == StateSettings.animationStates.isWalking;
            enemy.GetAnimator().WalkBlend();

            enemy.GetEnemyAgent().velocity = enemy.defaultVelocity;
            enemy.GetEnemyAgent().SetDestination(enemyPath.GetRandomPathPosition());
            enemy.GetEnemySO().fieldOfView = stateSettings.fieldOfView;
        }
        else if (stateSettings.currentActiveState is AttackState)
        {
            enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Attacking, stateSettings.animStates == StateSettings.animationStates.isAttacking);
            enemy.GetAnimator().AlertBlend();
        }
        else if (stateSettings.currentActiveState is SearchState)
        {
            enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Searching, stateSettings.animStates == StateSettings.animationStates.isRunning);
            enemy.GetAnimator().RunBlend();
            enemy.GetEnemyAgent().SetDestination(enemy.targetsLastPosition);
            enemy.GetEnemyAgent().velocity = enemy.defaultVelocity * 4f;
            enemy.GetEnemySO().fieldOfView = stateSettings.fieldOfView;
        }
        else if (stateSettings.currentActiveState is PossessedState)
        {
            enemy.GetEnemyAgent().velocity = UnityEngine.Vector3.zero;
            enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Possessed, stateSettings.animStates == StateSettings.animationStates.isPossessed);
            enemy.GetEnemyAgent().isStopped = true;
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
        else if (stateSettings.currentActiveState is PossessedState)
        {
            enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Possessed, false);
            enemy.GetAnimator().WalkBlend();
        }
    }
}

