using System.Collections;
using UnityEngine;

public class HealState : BaseState
{
    EventOnlyInteractable interactable;
    bool isHealing;
    Vector3 healingPosition;

    public override void Enter()
    {
        
        interactable = GameObject.FindGameObjectWithTag("Interactable").GetComponent<EventOnlyInteractable>();
        healingPosition = this.interactable.gameObject.transform.position;
        enemy.Agent.SetDestination(healingPosition);
        isHealing = false;
        enemy.fieldOfView = 180f;
    }

    public override void Perform()
    {
        if (!enemy.CanSeePlayer())
        {
            if (enemy.Agent.remainingDistance <= 1f && !isHealing)//if (enemy.Agent.remainingDistance < 0.2f)
            {
                enemy.StartCoroutine(Interacting(interactable));
            }
        }
        else
        {
            stateMachine.ChangeState(new FleeState()); // BUG keep on transitioning to Heal and Flee
        }
    }

    public override void Exit()
    {
        //enemy.anim.SetBool(Enemy.IS_IDLE, false); // should be heal animation
        enemy.Agent.velocity = enemy.defaultVelocity;
        enemy.StopAllCoroutines();
    }

    IEnumerator Interacting(Interactable _interactable)
    {
        isHealing = true;
        _interactable?.BaseInteract();
        enemy.anim.Play("Idle");// (Enemy.IS_IDLE, true); // Should be Heal Animation
        yield return new WaitForSeconds(10f);
        isHealing = false;
        stateMachine.ChangeState(new PatrolState());
    }
}

