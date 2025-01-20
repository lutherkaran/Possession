using UnityEngine;

public class AttackState : BaseState
{
    private float moveTimer;
    private float losePlayerTimer;
    private float shotTimer;

    public override void Enter()
    {
        enemy.anim.SetBool(Enemy.IS_ATTACKING, true);
        enemy.Agent.velocity = Vector3.zero;
    }

    public override void Exit()
    {
        enemy.anim.SetBool(Enemy.IS_ATTACKING, false);
        enemy.Agent.velocity = enemy.defaultVelocity;
        moveTimer = 0;
        losePlayerTimer = 0;
        shotTimer = 0;
    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer())
        {
            enemy.transform.LookAt(enemy.Player.transform);

            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;
            shotTimer += Time.deltaTime;

            if (shotTimer > enemy.fireRate)
            {
                Shoot();
            }
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
                enemy.LastKnownPos = enemy.Player.transform.position;
                stateMachine.ChangeState(new SearchState());
            }
        }
    }

    public void Shoot()
    {
        Transform gunBarrel = enemy.gunBarrel;
        GameObject bullet = GameObject.Instantiate(Resources.Load("Prefabs/Bullet") as GameObject, gunBarrel.position, enemy.transform.rotation);
        Vector3 shootDirection = (enemy.Player.transform.position - gunBarrel.transform.position).normalized;
        bullet.GetComponent<Rigidbody>().velocity = Quaternion.AngleAxis(Random.Range(-3f, 3f), Vector3.up) * shootDirection * 30f;
        //Debug.Log("Shoot");
        shotTimer = 0;
    }
}
