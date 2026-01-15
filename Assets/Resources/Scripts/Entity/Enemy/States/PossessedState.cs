using log4net;
using UnityEngine;

public class PossessedState : BaseState
{
    private Enemy enemy;
    private NPCAI npcAi;
    private Npc npc;

    private AnimalNpc animalNpc;

    private readonly float WalkSpeed = 10f;
    private Vector3 moveDirection;

    public PossessedState(NPCAI npcAi) : base(npcAi.gameObject)
    {
        this.npcAi = npcAi;

        if (npcAi is Enemy e)
            enemy = e;
        else if (npcAi is Npc n)
            npc = n;
        else if (npcAi is AnimalNpc a)
            animalNpc = a;
    }

    protected override void EnterState()
    {
        base.EnterState();

        if (animalNpc)
        {
            animalNpc.GetAnimalNpcAgent().velocity = Vector3.zero;
            animalNpc.GetAnimalNpcAgent().isStopped = true;

            Debug.Log(animalNpc.gameObject.name);
        }

        else if (enemy)
        {
            enemy.GetEnemyAgent().velocity = Vector3.zero;
            enemy.GetEnemyAgent().isStopped = true;
            enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Possessed, true);
            PossessionManager.instance.OnPossessed += Enemy_OnPossessed;
        }
    }

    private void Enemy_OnPossessed(object sender, IPossessable e)
    {
        enemy.GetAnimator().ManualBlend(WalkSpeed);
    }

    protected override void PerformState()
    {
        moveDirection.x = InputManager.instance.GetOnFootActions().Movement.ReadValue<Vector2>().x;
        moveDirection.z = InputManager.instance.GetOnFootActions().Movement.ReadValue<Vector2>().y;
        moveDirection.y = 0;

        if (PossessionManager.instance.GetCurrentPossessable() == enemy.possessedByPlayer)
            enemy.transform.Translate(moveDirection * WalkSpeed * Time.deltaTime);
        //    enemy.GetAnimator().ManualBlend(WalkSpeed * Time.deltaTime);
    }

    protected override void ExitState()
    {
        enemy.GetAnimator().SetAnimations(EnemyAnimator.AnimationStates.Possessed, false);
        enemy.GetAnimator().WalkBlend();
        stateMachine.ChangeState(stateMachine.lastActiveState);
    }

}
