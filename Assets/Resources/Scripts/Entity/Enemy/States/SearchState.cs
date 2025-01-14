using UnityEngine;

public class SearchState : BaseState
{
    private float searchTimer;
    private float moveTimer;

    public override void Enter()
    {
        enemy.Agent.SetDestination(enemy.LastKnownPos);
        enemy.anim.SetBool(Enemy.FLEE,true);
        enemy.anim.SetBool(Enemy.ATTACK,false);
        enemy.anim.SetBool(Enemy.PATROLLING,false);
        enemy.Agent.speed = 3;
    }

    public override void Perform()
    {
        if (enemy.GetHealth() > 30f)
        {
            if (enemy.CanSeePlayer())
                stateMachine.ChangeState(new AttackState());

            if (enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)
            {
                searchTimer += Time.deltaTime;
                moveTimer += Time.deltaTime;

                if (moveTimer > Random.Range(3f, 7f))
                {
                    enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 10f));
                    moveTimer = 0;
                    //stateMachine.ChangeState(new IdleState());
                }

                if (searchTimer > 7f)
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
        searchTimer = 0;
        moveTimer = 0;
        enemy.LastKnownPos = Vector3.zero;
    }
}
