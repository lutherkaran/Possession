using UnityEngine;

public class FleeState : BaseState
{
    Vector3 fleeDirection = Vector3.zero;
    float FleeDistance = 10f;

    public override void Enter()
    {
        enemy.anim.SetBool(Enemy.IS_FLEEING, true);
        enemy.Agent.velocity = enemy.defaultVelocity * 4f;
    }

    public override void Perform()
    {
        if (!enemy.IsSafe())
        {
            fleeDirection = (enemy.transform.position - enemy.Player.transform.position).normalized + (Random.insideUnitSphere * 10).normalized;
            enemy.Agent.SetDestination(enemy.transform.position + fleeDirection * FleeDistance);
            //stateMachine.ChangeState(new FleeState());
        }
        else
        {
            //heal
            if (!enemy.CanSeePlayer())
            {
                GameObject.FindGameObjectWithTag("Interactable").TryGetComponent(out EventOnlyInteractable interactable);
                stateMachine.ChangeState(new HealState(interactable, interactable.gameObject.transform.position));
            }
            else
            {
                if (enemy.GetHealth() > 30f)
                {
                    stateMachine.ChangeState(new AttackState());
                }
                else
                {
                    fleeDirection = (enemy.transform.position - enemy.Player.transform.position).normalized + (Random.insideUnitSphere * 10).normalized;
                    enemy.Agent.SetDestination(enemy.transform.position + fleeDirection * FleeDistance);
                    //stateMachine.ChangeState(new FleeState());
                }
            }
        }

    }

    public override void Exit()
    {
        enemy.anim.SetBool(Enemy.IS_FLEEING, false);
        enemy.Agent.velocity = enemy.defaultVelocity;
    }
}
