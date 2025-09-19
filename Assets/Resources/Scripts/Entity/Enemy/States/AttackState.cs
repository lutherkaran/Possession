using UnityEngine;

public class AttackState : BaseState
{
    private Enemy enemy;

    private float moveTimer;
    private float losePlayerTimer;

    public AttackState(Enemy _enemy) : base(_enemy.gameObject)
    {
        enemy = _enemy;
    }

    protected override void EnterState()
    {
        enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Attacking, true);
    }

    protected override void ExitState()
    {
        enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Attacking, false);
        enemy.Agent.velocity = enemy.defaultVelocity;
        moveTimer = 0;
        losePlayerTimer = 0;
    }

    protected override void PerformState()
    {
        if (enemy.CanSeePlayer())
        {
            enemy.transform.LookAt(enemy.player.transform);
            Shoot();

            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;

            if (moveTimer > Random.Range(3, 7))
            {
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 5));
                moveTimer = 0;
            }
        }

        else
        {
            losePlayerTimer += Time.deltaTime;

            if (losePlayerTimer > 5)
            {
                enemy.LastKnownPos = enemy.player.transform.position;
                stateMachine.ChangeState(new SearchState(enemy));
            }
        }
    }

    private void Shoot()
    {
        Transform gunBarrel = enemy.gunBarrel;
        Vector3 shootDirection = (enemy.player.transform.position - gunBarrel.transform.position).normalized;
        Bullet.Shoot(enemy, enemy.gunBarrel, shootDirection);
    }
}
