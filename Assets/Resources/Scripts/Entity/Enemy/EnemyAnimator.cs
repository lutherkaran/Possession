using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    public enum AnimationStates
    {
        Idle, Patrolling, Running, Searching, Attacking, Fleeing
    };

    private const string IS_IDLE = "IsIdle";
    private const string IS_PATROLLING = "IsPatrolling";
    private const string IS_SEARCHING = "IsSearching";
    private const string IS_RUNNING = "IsRunning";
    private const string IS_ATTACKING = "IsAttacking";
    private const string IS_FLEEING = "IsFleeing";

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
        { AnimationStates.Fleeing, IS_FLEEING }
     };
    }

    private void Awake()
    {
        enemyAnimator = GetComponent<Animator>();

        InitializingAnimationStatesDictionary();
    }

    public void SetAnimations(AnimationStates state, bool value)
    {
        if (animationStatesDictionary.TryGetValue(state, out string param))
        {
            enemyAnimator.SetBool(param, value);
        }
    }
}
