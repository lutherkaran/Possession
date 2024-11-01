using UnityEngine;

public class AttackState : BaseState
{
    private float moveTimer;
    private float losePlayerTimer;
    private float shotTimer;

    public override void Enter()
    {

    }

    public override void Exit()
    {
        moveTimer = 0;
        losePlayerTimer = 0;
        shotTimer = 0;
    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer())
        {
            losePlayerTimer = 0;

            moveTimer += Time.deltaTime;
            shotTimer += Time.deltaTime;

            enemy.transform.LookAt(enemy.Player.transform);

            //Vector3 direction = enemy.Player.transform.position - enemy.transform.position;
            //Quaternion targetRotation = Quaternion.LookRotation(direction);
            //enemy.transform.localRotation = Quaternion.Slerp(enemy.transform.localRotation, targetRotation, Time.deltaTime * rotationSpeed);

            if (shotTimer > enemy.fireRate)
            {
                Shoot();
            }

            if (moveTimer > Random.Range(3, 7))
            {
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 5));
                moveTimer = 0;
            }
            enemy.LastKnownPos = enemy.Player.transform.position;
        }

        else
        {
            losePlayerTimer += Time.deltaTime;

            if (losePlayerTimer > 5)
            {
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
