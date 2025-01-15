using System.Collections;
using UnityEngine;

public class SearchState : BaseState
{
    private float searchTimer;
    private float searchDuration = 10f;
    private float moveTimer;
    private bool isIdling = false;

    public override void Enter()
    {
        enemy.anim.SetBool(Enemy.FLEE, true);
        enemy.Agent.SetDestination(enemy.LastKnownPos);
        enemy.Agent.velocity = enemy.defaultVelocity * 2f;
        isIdling = false;
    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer())
            stateMachine.ChangeState(new AttackState());

        if (enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance && !isIdling)
        {
            enemy.StartCoroutine(IdleAndPickRandomLocation());

            searchTimer += Time.deltaTime;
            if (searchTimer > searchDuration)
            {
                stateMachine.ChangeState(new PatrolState());
            }
        }
    }

    public override void Exit()
    {
        enemy.anim.SetBool(Enemy.FLEE, false);
        searchTimer = 0;
        enemy.LastKnownPos = Vector3.zero;
        enemy.StopAllCoroutines();
    }

    private IEnumerator IdleAndPickRandomLocation()
    {
        isIdling = true;
        stateMachine.ChangeState(new IdleState());
        yield return new WaitForSeconds(5f);
        isIdling = false;

        stateMachine.ChangeState(new SearchState());
        enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 10f));
    }
}
