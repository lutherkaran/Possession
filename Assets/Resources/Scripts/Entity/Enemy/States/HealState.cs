using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealState : BaseState
{
    EventOnlyInteractable interactable;

    public override void Enter()
    {
        enemy.anim.SetBool(Enemy.IDLE, true); // Should be Heal Animation
        GameObject.FindGameObjectWithTag("Interactable").TryGetComponent(out interactable);
        enemy.Agent.SetDestination(interactable.gameObject.transform.position);
    }

    public override void Perform()
    {
        if (!enemy.CanSeePlayer())
        {
            if (enemy.Agent.remainingDistance == enemy.Agent.stoppingDistance)//if (enemy.Agent.remainingDistance < 0.2f)
            {
                enemy.Agent.velocity = Vector3.zero;
                interactable.BaseInteract();
                Debug.Log("Interacted Enemy....!!");
                if (enemy.GetHealth() >= 100)
                {
                    stateMachine.ChangeState(new PatrolState());
                }
            }
        }
        else
        {
            stateMachine.ChangeState(new FleeState());
        }
    }

    public override void Exit()
    {
        enemy.anim.SetBool(Enemy.IDLE, false); // should be heal animation
        enemy.Agent.velocity = enemy.defaultVelocity;
    }
}

