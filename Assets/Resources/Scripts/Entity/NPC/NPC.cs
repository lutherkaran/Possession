using UnityEngine;
using UnityEngine.AI;

public class Npc : Entity, IPossessable
{
    private NavMeshAgent npcAgent;

    protected float currentFixedDeltaTime = 0f;

    public virtual void Initialize()
    {
    }

    public virtual void PostInitialize()
    {
    
    }

    public virtual void Refresh(float deltaTime)
    {

    }

    public virtual void PhysicsRefresh(float fixedDeltaTime)
    {
  
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

    public virtual void Possessing(GameObject go)
    {
        possessedByPlayer = PossessionManager.instance.GetCurrentPossessable();
    }

    public virtual void Depossessing(GameObject go)
    {
        possessedByPlayer = null;
    }

    public Entity GetPossessedEntity() => this;

    public override Transform GetCameraAttachPoint() => cameraAttachPoint;

    public override float GetEntityPossessionTimerMax() => entitySO.entityPossessionTimerMax;

    public override float GetPossessionCooldownTimerMax() => entitySO.possessionCooldownTimerMax;
}
