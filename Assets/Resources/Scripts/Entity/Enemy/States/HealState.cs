using System.Collections;
using UnityEngine;

public class HealState : BaseState
{
    private Enemy enemy;

    private EventOnlyInteractable interactable;
    private bool isHealing;
    private Vector3 healingPosition;

    public HealState(Enemy _enemy) : base(_enemy.gameObject)
    {
        enemy = _enemy;
    }

    protected override void EnterState()
    {
        interactable = GameObject.FindGameObjectWithTag("Interactable").GetComponent<EventOnlyInteractable>();
        healingPosition = this.interactable.gameObject.transform.position;
        enemy.Agent.SetDestination(healingPosition);
        enemy.fieldOfView = 180f;
    }

    protected override void PerformState()
    {
        if (enemy.Agent.remainingDistance <= 1f && !isHealing)//if (enemy.Agent.remainingDistance < 0.2f)
        {
            enemy.StartCoroutine(Interacting(interactable));
        }
    }

    protected override void ExitState()
    {
        enemy.Agent.velocity = enemy.defaultVelocity;
        enemy.StopAllCoroutines();
        isHealing = false;
        interactable = null;
    }

    private IEnumerator Interacting(Interactable _interactable)
    {
        enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Idle, true);
        isHealing = true;
        _interactable?.BaseInteract();
        yield return new WaitForSeconds(5f);

        enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Idle, false);
        stateMachine.ChangeState(new PatrolState(enemy));
    }
}

