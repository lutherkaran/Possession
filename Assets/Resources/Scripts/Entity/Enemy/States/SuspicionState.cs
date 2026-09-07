using UnityEngine;

public class SuspicionState : BaseState
{
    private readonly Enemy enemy;

    private float reactionTimer;
    private readonly float reactionWindow = 2.5f;

    private float loseTargetTimer;
    private readonly float loseTargetGrace = 1.5f;

    private readonly float captureRadius = 1.5f;

    private new readonly IStateContext stateContext;

    public SuspicionState(IStateContext stateContext) : base(stateContext)
    {
        this.stateContext = stateContext;
        enemy = this.stateContext as Enemy;
    }

    protected override void EnterState()
    {
        stateMachine.GetCurrentStateSettings().UpdateSettings(StateSettings.animationStates.isRunning, Vector3.zero, 150f);
        stateContext.ApplySettings(stateMachine.GetCurrentStateSettings());
        reactionTimer = 0f;
        loseTargetTimer = 0f;
    }

    protected override void PerformState()
    {
        if (enemy == null) return;

        if (stateContext.CanSeePossessedAnimal())
        {
            loseTargetTimer = 0f;
            reactionTimer += Time.deltaTime;

            Transform target = PossessionManager.instance.GetActiveTransform();
            if (target != null)
            {
                enemy.GetEnemyAgent().SetDestination(target.position);

                float distance = Vector3.Distance(enemy.transform.position, target.position);
                if (distance <= captureRadius || reactionTimer >= reactionWindow)
                {
                    CaptureAnimal();
                }
            }
        }
        else
        {
            loseTargetTimer += Time.deltaTime;
            if (loseTargetTimer >= loseTargetGrace)
            {
                stateMachine.ChangeState(stateMachine.GetAvailableStates()[typeof(IdleState)]);
            }
        }
    }

    protected override void ExitState()
    {
        stateContext.ResetChanges();
    }

    private void CaptureAnimal()
    {
        var capturedPossessable = PossessionManager.instance.GetCurrentPossessable();

        AlertManager.instance?.ReportAnimalCaptured();
        PossessionManager.instance.ToPossess(PlayerManager.instance.GetPlayer().gameObject);

        if (capturedPossessable is AnimalNpc animalNpc)
        {
            animalNpc.OnCaptured();
        }

        stateMachine.ChangeState(stateMachine.GetAvailableStates()[typeof(IdleState)]);
    }
}