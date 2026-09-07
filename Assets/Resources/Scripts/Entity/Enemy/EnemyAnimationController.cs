public class EnemyAnimationController
{
    private readonly Enemy enemy;

    public EnemyAnimationController(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void RunAI(StateSettings stateSettings)
    {
        BaseState activeState = enemy.GetStateMachine().currentActiveState;

        if (activeState is IdleState)
        {
            enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Idle, stateSettings.animStates == StateSettings.animationStates.isIdle);
            enemy.GetEnemyAgent().velocity = stateSettings.desiredVelocity;
            enemy.GetAnimator().ResetBlend();
            enemy.GetEnemyAgent().isStopped = stateSettings.animStates == StateSettings.animationStates.isIdle;
        }
        else if (activeState is PatrolState)
        {
            EnemyManager.instance.enemyPathEnemyDictionary.TryGetValue(enemy, out EnemyPath enemyPath);

            enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Patrolling, stateSettings.animStates == StateSettings.animationStates.isWalking);
            enemy.GetEnemyAgent().isStopped = stateSettings.animStates == StateSettings.animationStates.isWalking;
            enemy.GetAnimator().WalkBlend();

            enemy.GetEnemyAgent().velocity = enemy.defaultVelocity;
            enemy.GetEnemyAgent().SetDestination(enemyPath.GetRandomPathPosition());
        }
        else if (activeState is AttackState)
        {
            enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Attacking, stateSettings.animStates == StateSettings.animationStates.isAttacking);
            enemy.GetAnimator().AlertBlend();
        }
        else if (activeState is SuspicionState)
        {
            enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Attacking, stateSettings.animStates == StateSettings.animationStates.isRunning);
            enemy.GetAnimator().AlertBlend();
        }
        else if (activeState is SearchState)
        {
            enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Searching, stateSettings.animStates == StateSettings.animationStates.isRunning);
            enemy.GetAnimator().RunBlend();
            enemy.GetEnemyAgent().SetDestination(enemy.targetsLastPosition);
            enemy.GetEnemyAgent().velocity = enemy.defaultVelocity * 4f;
        }
        else if (activeState is PossessedState)
        {
            enemy.GetEnemyAgent().velocity = UnityEngine.Vector3.zero;
            enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Possessed, stateSettings.animStates == StateSettings.animationStates.isPossessed);
            enemy.GetEnemyAgent().isStopped = true;
            enemy.GetEnemyAgent().enabled = false;

            if (enemy.GetRigidBody() != null)
                enemy.GetRigidBody().isKinematic = false;
        }
    }

    public void Reset()
    {
        BaseState activeState = enemy.GetStateMachine().currentActiveState;

        switch (activeState)
        {
            case IdleState:
                enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Idle);
                break;
            case PatrolState:
                enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Patrolling);
                break;
            case AttackState:
                enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Attacking);
                enemy.GetEnemyAgent().velocity = enemy.defaultVelocity;
                break;
            case SuspicionState:
                enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Attacking);
                enemy.GetEnemyAgent().velocity = enemy.defaultVelocity;
                break;
            case SearchState:
                enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Searching);
                enemy.GetEnemyAgent().velocity = enemy.defaultVelocity;
                break;
            case PossessedState:
                enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Possessed);
                if (enemy.GetRigidBody() != null)
                {
                    enemy.GetRigidBody().isKinematic = true;
                }
                enemy.GetEnemyAgent().enabled = true;
                break;
        }
    }
}