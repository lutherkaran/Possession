using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Npc : NPCAI, IPossessable
{
    private float currentFixedDeltaTime = 0f;

    protected Dictionary<Type, BaseState> animalStates;
    protected StateMachine npcStateMachine;

    public virtual void Initialize()
    {
        npcStateMachine = GetComponent<StateMachine>();
    }

    public virtual void PostInitialize()
    {
        InitializeAnimalStateDictionary();
    }

    private void InitializeAnimalStateDictionary()
    {
        animalStates = new Dictionary<Type, BaseState>()
        {
            { typeof(IdleState), new IdleState(this) },
            { typeof(PatrolState), new PatrolState(this) },
            { typeof(PossessedState), new PossessedState(this) }
        };

        npcStateMachine.Initialise(this, animalStates);
    }

    public virtual void Refresh(float deltaTime)
    {
        npcStateMachine.Refresh(deltaTime);
    }

    public virtual void PhysicsRefresh(float fixedDeltaTime)
    {
        currentFixedDeltaTime = fixedDeltaTime;
    }

    public virtual void LateRefresh(float deltaTime)
    {

    }

    public virtual void OnDemolish()
    {

    }

    public override void Attack()
    {

    }

    protected override bool IsAlive()
    {
        return false;
    }

    public override void MoveWhenPossessed(Vector2 input)
    {
        base.MoveWhenPossessed(input);
        if (PossessionManager.instance.GetCurrentPossessable() == possessedByPlayer)
            transform.Translate(moveDirection * entitySO.speed * currentFixedDeltaTime);
    }

    public override void ProcessJump()
    {
        base.ProcessJump();
        if (PossessionManager.instance.GetCurrentPossessable() == possessedByPlayer)
        {
            transform.position += velocity * currentFixedDeltaTime;
            velocity.y = gravity * currentFixedDeltaTime * 10;
        }
    }

    public override void Sprint()
    {
        base.Sprint();
    }

    public void Possessing(GameObject go)
    {
        possessedByPlayer = PossessionManager.instance.GetCurrentPossessable();
        npcStateMachine.ChangeState(new PossessedState(this));
    }

    public void Depossessing(GameObject go)
    {
        npcStateMachine.ChangeState(new IdleState(this));
        possessedByPlayer = null;
    }

    protected override void ApplyChanges(BaseState a)
    {
        base.ApplyChanges(a);
    }

    public Entity GetPossessedEntity() => this;

    public override Transform GetCameraAttachPoint() => cameraAttachPoint;

    public override float GetEntityPossessionTimerMax() => entitySO.entityPossessionTimerMax;

    public override float GetPossessionCooldownTimerMax() => entitySO.possessionCooldownTimerMax;
}
