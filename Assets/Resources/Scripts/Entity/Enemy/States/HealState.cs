using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealState : BaseState
{
    public override void Enter()
    {

    }

    public override void Perform()
    {
        // Find heal potion 
        if (enemy.GetHealth() < 50)
        {
            GameObject.FindGameObjectWithTag("Interactable").TryGetComponent(out EventOnlyInteractable interactable);
            enemy.Agent.SetDestination(interactable.gameObject.transform.position);

            if (enemy.Agent.remainingDistance < 0.2f)
            {
                interactable.BaseInteract();

            }

            if (enemy.GetHealth() >= 100)
            {
                stateMachine.ChangeState(new PatrolState());
            }
        }

    }

    public override void Exit()
    {

    }
}

