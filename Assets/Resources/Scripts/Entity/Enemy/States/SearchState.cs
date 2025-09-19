using System.Collections;
using UnityEngine;

public class SearchState : BaseState
{
    private Enemy enemy;

    private float searchTimer;
    private float moveTimer;
    private bool isSettingIdle;
    private float searchDuration = 10f;

    public SearchState(Enemy _enemy): base(_enemy.gameObject)
    {
        enemy = _enemy;
    }

    protected override void EnterState()
    {
        enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Searching, true);

        enemy.Agent.SetDestination(enemy.LastKnownPos);
        enemy.Agent.velocity = enemy.defaultVelocity * 2f;
        enemy.fieldOfView = 180f;
        isSettingIdle = false;
    }

    protected override void PerformState()
    {
        if (enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState(enemy));
        }

        if (enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)
        {
            searchTimer += Time.deltaTime;
            moveTimer += Time.deltaTime;
            if (moveTimer >= Random.Range(3f, searchDuration - 1) && !isSettingIdle)
            {
                enemy.StartCoroutine(SettingUpIdle());
                moveTimer = 0;
            }
        }

        if (searchTimer > searchDuration)
        {
            stateMachine.ChangeState(new PatrolState(enemy));
        }
    }

    protected override void ExitState()
    {
        enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Searching, false);

        searchTimer = 0;
        moveTimer = 0;
        isSettingIdle = false;
        enemy.LastKnownPos = Vector3.zero;
        enemy.Agent.SetDestination(enemy.transform.position); // Clear destination
        enemy.StopAllCoroutines();
    }

    private IEnumerator SettingUpIdle()
    {
        isSettingIdle = true;
        enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Idle, true);

        yield return new WaitForSeconds(3f);
        isSettingIdle = false; // Allow future idles
        enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 30f));
    }
}
