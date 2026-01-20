using UnityEngine;

public class AttackState : BaseState
{
    private Enemy enemy;

    private StateSettings settings;

    private float moveTimer;
    private float losePlayerTimer;

    public AttackState(IStateContext _stateContext) : base(_stateContext)
    {
        stateContext = _stateContext;
        settings = new StateSettings(stateContext, this, true, false, false, Vector3.zero, 150f);
    }

    protected override void EnterState()
    {
        stateContext.ApplySettings(settings);
    }

    protected override void ExitState()
    {
        stateContext.ResetChanges();
    }

    protected override void PerformState()
    {
        if (stateContext.CanSeePlayer())
        {
            enemy.transform.LookAt(enemy.GetTargetPlayerTransform());

            // lock the aim towards the player.
            MoveRandomlyInCirle();
        }

        else
        {
            losePlayerTimer += Time.deltaTime;

            if (losePlayerTimer > 3)
            {
                // Calls all the enemies to current position or sorrounds
                // Then Alert them
                //stateMachine.ChangeState(new SearchState(enemy));
                losePlayerTimer = 0;
            }
        }
    }

    private void MoveRandomlyInCirle()
    {
        moveTimer += Time.deltaTime;

        if (moveTimer > Random.Range(3, 7))
        {
            enemy.GetEnemyAgent().SetDestination(enemy.transform.position + (Random.insideUnitSphere * 5));
            moveTimer = 0;
        }
    }

    // Move towards the player position, aim towards him, and alert other enemies or call them there.
    // if Enemy lost player then he starts searching, then he also alert and calls other enemies.
}
