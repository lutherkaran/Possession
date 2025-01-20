using System.Collections;
using UnityEngine;

public class SearchState : BaseState
{
    private float searchTimer;
    private float moveTimer;
    private bool isSettingIdle;
    private float searchDuration = 10f;

    public override void Enter()
    {
        enemy.anim.SetBool(Enemy.IS_SEARCHING, true);
        enemy.Agent.SetDestination(enemy.LastKnownPos);
        enemy.Agent.velocity = enemy.defaultVelocity * 2f;
        enemy.fieldOfView = 180f;
        isSettingIdle = false;
    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
            return;
        }

        if (enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)
        {
            searchTimer += Time.deltaTime;

            if (!isSettingIdle)
            {
                moveTimer += Time.deltaTime;
                if (moveTimer >= Random.Range(3f, searchDuration - 1))
                {
                    isSettingIdle = true;
                    enemy.StartCoroutine(SettingUpIdle());
                    moveTimer = 0;
                }
            }
        }

        if (searchTimer > searchDuration)
        {
            stateMachine.ChangeState(new PatrolState());
        }
    }

    public override void Exit()
    {
        enemy.anim.SetBool(Enemy.IS_SEARCHING, false);
        searchTimer = 0;
        moveTimer = 0;
        isSettingIdle = false;
        enemy.LastKnownPos = Vector3.zero;
        enemy.Agent.SetDestination(enemy.transform.position); // Clear destination
        enemy.StopAllCoroutines();
    }

    private IEnumerator SettingUpIdle()
    {
        yield return new WaitForSeconds(3f);
        isSettingIdle = false; // Allow future idles
        enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 30f));
    }
}
