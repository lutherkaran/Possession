using UnityEngine;

public class PossessedState : BaseState
{
    private Enemy enemy;

    private readonly float WalkSpeed = 10f;
    private Vector3 moveDirection;

    public PossessedState(Enemy _enemy) : base(_enemy.gameObject)
    {
        enemy = _enemy;
    }

    protected override void EnterState()
    {
        enemy.GetEnemyAgent().velocity = Vector3.zero;
        enemy.GetEnemyAgent().isStopped = true;
        // play possessed Animation..
    }

    protected override void PerformState()
    {
        moveDirection.x = InputManager.Instance.GetOnFootActions().Movement.ReadValue<Vector2>().x;
        moveDirection.z = InputManager.Instance.GetOnFootActions().Movement.ReadValue<Vector2>().y;
        moveDirection.y = 0;

        if (PossessionManager.Instance.GetCurrentPossessable() == enemy.possessedByPlayer)
            enemy.transform.Translate(moveDirection * WalkSpeed * Time.deltaTime);
    }

    protected override void ExitState()
    {
        enemy.GetAnimator().WalkBlend();
        stateMachine.ChangeState(stateMachine.lastActiveState);
    }

}
