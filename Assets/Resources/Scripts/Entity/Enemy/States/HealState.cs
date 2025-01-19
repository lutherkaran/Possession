using System.Collections;
using UnityEngine;

public class HealState : BaseState
{
    EventOnlyInteractable interactable;
    bool isHealing;
    Vector3 healingPosition;

    public HealState(EventOnlyInteractable interactable, Vector3 targetPosition)
    {
        this.interactable = interactable;
        this.healingPosition = targetPosition;
    }

    public override void Enter()
    {
        isHealing = false;
    }

    public override void Perform()
    {
        if (!enemy.CanSeePlayer())
        {
            enemy.Agent.SetDestination(healingPosition);
            if (enemy.Agent.remainingDistance <= 1f && !isHealing)//if (enemy.Agent.remainingDistance < 0.2f)
            {
                enemy.StartCoroutine(Interacting());
            }
        }

        else
        {
            stateMachine.ChangeState(new FleeState());
        }
    }

    public override void Exit()
    {
        enemy.anim.SetBool(Enemy.IS_IDLE, false); // should be heal animation
        enemy.Agent.velocity = enemy.defaultVelocity;
        enemy.StopAllCoroutines();
    }
    IEnumerator Interacting()
    {
        isHealing = true;
        interactable.BaseInteract();
        enemy.anim.SetBool(Enemy.IS_IDLE, true); // Should be Heal Animation
        yield return new WaitForSeconds(3f);
        isHealing = false;
        stateMachine.ChangeState(new PatrolState());
    }
}

