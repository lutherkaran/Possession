using NUnit.Framework.Constraints;
using UnityEngine;

public class Npc : Entity, IPossessable
{
    protected float currentFixedDeltaTime = 0f;

    protected float actualSpeed { get; private set; } = 0f;

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

    public override EntityAnimation GetEntityAnimation() => entityAnimation;

    public override float GetEntityPossessionTimerMax() => entitySO.entityPossessionTimerMax;

    public override float GetPossessionCooldownTimerMax() => entitySO.possessionCooldownTimerMax;

    public float GetActualSpeed() => actualSpeed;
}
