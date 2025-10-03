using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    public enum AnimationStates
    {
        Idle, Patrolling, Running, Searching, Attacking, Fleeing, Possessed
    };

    private const string IS_IDLE = "IsIdle";
    private const string IS_PATROLLING = "IsPatrolling";
    private const string IS_SEARCHING = "IsSearching";
    private const string IS_RUNNING = "IsRunning";
    private const string IS_ATTACKING = "IsAttacking";
    private const string IS_FLEEING = "IsFleeing";
    private const string IS_POSSESSED = "IsPossessed";

    private Enemy enemy;
    private Animator enemyAnimator;

    private Dictionary<AnimationStates, string> animationStatesDictionary;


    private void InitializingAnimationStatesDictionary()
    {
        animationStatesDictionary = new Dictionary<AnimationStates, string>()
     {
        { AnimationStates.Idle, IS_IDLE },
        { AnimationStates.Patrolling, IS_PATROLLING },
        { AnimationStates.Running, IS_RUNNING},
        { AnimationStates.Searching, IS_SEARCHING },
        { AnimationStates.Attacking, IS_ATTACKING },
        { AnimationStates.Fleeing, IS_FLEEING },
        { AnimationStates.Possessed, IS_POSSESSED }
     };
    }

    private void Awake()
    {
        enemyAnimator = GetComponent<Animator>();
        enemy = GetComponentInParent<Enemy>();

        InitializingAnimationStatesDictionary();
    }

    public void SetAnimations(AnimationStates state, bool value)
    {
        if (animationStatesDictionary.TryGetValue(state, out string param))
        {
            enemyAnimator.SetBool(param, value);
        }
    }

    public void Shoot()
    {
        BulletManager.instance.Shoot(enemy, enemy.GetGunBarrelTransform(), enemy.shootDirection);
    }

    public void AttackingBlend()
    {
        enemyAnimator.SetFloat("Blend", 0.3f);
        enemyAnimator.SetLayerWeight(1, 1f);
    }

    public void RunBlend()
    {
        enemyAnimator.SetFloat("Blend", 2f);
        enemyAnimator.SetLayerWeight(1, 0f);
    }

    public void WalkBlend()
    {
        enemyAnimator.SetFloat("Blend", .5f);
        enemyAnimator.SetLayerWeight(1, 0f);
    }

    public void ResetBlend()
    {
        enemyAnimator.SetFloat("Blend", .0f);
        enemyAnimator.SetLayerWeight(1, 0f);
    }

    public void ManualBlend(float value)
    {
        enemyAnimator.SetFloat("Blend", value);
        enemyAnimator.SetLayerWeight(1, 0f);

    }
}
