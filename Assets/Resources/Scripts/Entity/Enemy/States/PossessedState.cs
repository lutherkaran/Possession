using UnityEngine;

public class PossessedState : BaseState
{
    private readonly float WalkSpeed = 10f;
    private Vector3 moveDirection;

    private Enemy enemy;

    public PossessedState(Enemy _enemy) : base(_enemy.gameObject)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        enemy.Agent.velocity = Vector3.zero;
        enemy.Agent.isStopped = true;
    }

    public override void Perform()
    {
        moveDirection.x = InputManager.Instance.GetOnFootActions().Movement.ReadValue<Vector2>().x;
        moveDirection.z = InputManager.Instance.GetOnFootActions().Movement.ReadValue<Vector2>().y;
        moveDirection.y = 0;

        if (PossessionManager.Instance.GetCurrentPossessable() == enemy.possessedByPlayer)
            enemy.transform.Translate(moveDirection * WalkSpeed * Time.deltaTime);
    }

    public override void Exit()
    {

    }

}
