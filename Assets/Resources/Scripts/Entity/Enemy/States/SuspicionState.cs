using UnityEngine;

// Low-stakes threat: a guard notices the player's *currently possessed animal* behaving
// unusually and moves to cage/capture it. Distinct from AttackState (the abandoned-body
// threat), which is the high-stakes, game-over-capable branch.
public class SuspicionState : BaseState
{
    private readonly Enemy enemy;

    private float reactionTimer;
    private readonly float reactionWindow = 2.5f; // grace period before capture -- break line of sight to escape

    private float loseTargetTimer;
    private readonly float loseTargetGrace = 1.5f; // forgiveness if the animal briefly ducks out of view

    private readonly float captureRadius = 1.5f;

    private new readonly IStateContext stateContext;

    public SuspicionState(IStateContext stateContext) : base(stateContext)
    {
        this.stateContext = stateContext;
        enemy = this.stateContext as Enemy;
    }

    protected override void EnterState()
    {
        stateMachine.GetCurrentStateSettings().UpdateSettings(this.stateContext, this, StateSettings.animationStates.isRunning, Vector3.zero, 150f);
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
                // Animal broke line of sight in time -- guard gives up and resumes normal duty.
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