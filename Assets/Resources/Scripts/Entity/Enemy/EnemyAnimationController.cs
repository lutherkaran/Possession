public class EnemyAnimationController
{
    private readonly Enemy enemy;
    private StateSettings stateSettings;

    public EnemyAnimationController(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void RunAI(StateSettings stateSettings)
    {
        this.stateSettings = stateSettings;

        if (this.stateSettings.currentActiveState is IdleState)
        {
            enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Idle, this.stateSettings.animStates == StateSettings.animationStates.isIdle);
            enemy.GetEnemyAgent().velocity = this.stateSettings.desiredVelocity;
            enemy.GetAnimator().ResetBlend();
            enemy.GetEnemyAgent().isStopped = this.stateSettings.animStates == StateSettings.animationStates.isIdle;
        }
        else if (this.stateSettings.currentActiveState is PatrolState)
        {
            EnemyManager.instance.enemyPathEnemyDictionary.TryGetValue(enemy, out EnemyPath enemyPath);

            enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Patrolling, this.stateSettings.animStates == StateSettings.animationStates.isWalking);
            enemy.GetEnemyAgent().isStopped = this.stateSettings.animStates == StateSettings.animationStates.isWalking;
            enemy.GetAnimator().WalkBlend();

            enemy.GetEnemyAgent().velocity = enemy.defaultVelocity;
            enemy.GetEnemyAgent().SetDestination(enemyPath.GetRandomPathPosition());
        }
        else if (this.stateSettings.currentActiveState is AttackState)
        {
            enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Attacking, this.stateSettings.animStates == StateSettings.animationStates.isAttacking);
            enemy.GetAnimator().AlertBlend();
        }
        else if (this.stateSettings.currentActiveState is SuspicionState)
        {
            // Reuses the Attacking/alert blend for now -- swap for a dedicated "suspicious" animation
            // state when art has one; functionally this is a distinct, lower-stakes threat.
            enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Attacking, this.stateSettings.animStates == StateSettings.animationStates.isRunning);
            enemy.GetAnimator().AlertBlend();
        }
        else if (this.stateSettings.currentActiveState is SearchState)
        {
            enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Searching, this.stateSettings.animStates == StateSettings.animationStates.isRunning);
            enemy.GetAnimator().RunBlend();
            enemy.GetEnemyAgent().SetDestination(enemy.targetsLastPosition);
            enemy.GetEnemyAgent().velocity = enemy.defaultVelocity * 4f;
        }
        else if (this.stateSettings.currentActiveState is PossessedState)
        {
            enemy.GetEnemyAgent().velocity = UnityEngine.Vector3.zero;
            enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Possessed, this.stateSettings.animStates == StateSettings.animationStates.isPossessed);
            enemy.GetEnemyAgent().isStopped = true;
            enemy.GetEnemyAgent().enabled = false; // hand full control to the Rigidbody while possessed

            if (enemy.GetRigidBody() != null)
                enemy.GetRigidBody().isKinematic = false;
        }
    }

    public void Reset()
    {
        switch (this.stateSettings.currentActiveState)
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
