using System.Collections;
using UnityEngine;

public class SearchState : BaseState
{
    private float searchTimer;
    private float searchDuration = 60f;
    private float moveTimer;
    private bool isSearching = false;

    public override void Enter()
    {
        enemy.anim.SetBool(Enemy.IS_SEARCHING, true);
        enemy.Agent.SetDestination(enemy.LastKnownPos);
        enemy.Agent.velocity = enemy.defaultVelocity * 2f;
        isSearching = true;
    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer())
            stateMachine.ChangeState(new AttackState());

        if (enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance && !isSearching)
        {
            searchTimer += Time.deltaTime;
            moveTimer += Time.deltaTime;
            if (moveTimer <= Random.Range(3f, searchDuration - 1))
            {
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 60f));
                if (enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)
                {
                    stateMachine.ChangeState(new IdleState());
                }
            }
        }
        else if (searchTimer > searchDuration)
            stateMachine.ChangeState(new PatrolState());
    }

    public override void Exit()
    {
        enemy.anim.SetBool(Enemy.IS_SEARCHING, false);
        searchTimer = 0;
        moveTimer = 0;
        enemy.LastKnownPos = Vector3.zero;
        enemy.StopAllCoroutines();
    }
}
