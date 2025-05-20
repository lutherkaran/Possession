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
        enemy.fieldOfView = 180f;
    }

    public override void Perform()
    {
        if (enemy.Agent.remainingDistance <= 1f && !isHealing)//if (enemy.Agent.remainingDistance < 0.2f)
        {
            enemy.StartCoroutine(Interacting(interactable));
        }
    }

    public override void Exit()
    {
        enemy.Agent.velocity = enemy.defaultVelocity;
        enemy.StopAllCoroutines();
        isHealing = false;
        interactable = null;
    }

    IEnumerator Interacting(Interactable _interactable)
    {
        isHealing = true;
        _interactable?.BaseInteract();
        enemy.GetAnimator().Play("Idle");// (Enemy.IS_IDLE, true); // Should be Heal Animation
        yield return new WaitForSeconds(10f);
        stateMachine.ChangeState(new PatrolState());
    }
}

