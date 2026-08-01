using UnityEngine;

// High-stakes threat: a guard has spotted the *abandoned human body*. Gives the player a short
// reaction window to break line of sight before escalating to a full alert (other guards called
// in, danger meter starts climbing via AlertManager -- reaching max ends the game).
public class AttackState : BaseState
{
    private Enemy enemy;
    private StateSettings settings;

    private float reactionTimer;
    private readonly float reactionWindow = 2.5f; // time to break line of sight after being spotted
    private bool hasAlertedOthers;

    private float losePlayerTimer;
    private readonly float losePlayerGrace = 3f;

    private float moveTimer;

    public AttackState(IStateContext _stateContext) : base(_stateContext)
    {
        stateContext = _stateContext;
        enemy = stateContext as Enemy;
        settings = new StateSettings(stateContext, this, StateSettings.animationStates.isAttacking, Vector3.zero, 150f);
    }

    protected override void EnterState()
    {
        stateContext.ApplySettings(settings);
        reactionTimer = 0f;
        losePlayerTimer = 0f;
        hasAlertedOthers = false;

        if (enemy != null)
            AlertManager.instance?.ReportBodySpotted(enemy.transform.position);
    }

    protected override void ExitState()
    {
        stateContext.ResetChanges();
    }

    protected override void PerformState()
    {
        if (enemy == null) return;

        if (stateContext.CanSeePossessedPlayer())
        {
            losePlayerTimer = 0f;
            enemy.transform.LookAt(enemy.GetTargetPlayerTransform());

            MoveRandomlyInCirle();

            reactionTimer += Time.deltaTime;

            if (!hasAlertedOthers && reactionTimer >= reactionWindow)
            {
                hasAlertedOthers = true;
                EnemyManager.instance.AlertAllGuards(enemy.GetTargetPlayerTransform().position, enemy);
                AlertManager.instance?.EscalateAlert();
            }

            if (hasAlertedOthers)
            {
                AlertManager.instance?.RaiseDanger(Time.deltaTime * 0.15f);
            }
        }
        else
        {
            losePlayerTimer += Time.deltaTime;

            if (losePlayerTimer > losePlayerGrace)
            {
                AlertManager.instance?.ReportBodyLost();
                stateMachine.ChangeState(new SearchState(stateContext));
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
}