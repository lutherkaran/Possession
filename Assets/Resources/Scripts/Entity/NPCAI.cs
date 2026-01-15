using Codice.Client.BaseCommands;
using UnityEngine;

public class NPCAI : Entity
{
    public override void Attack()
    {
        throw new System.NotImplementedException();
    }

    public override Transform GetCameraAttachPoint()
    {
        throw new System.NotImplementedException();
    }

    public override float GetEntityPossessionTimerMax()
    {
        throw new System.NotImplementedException();
    }

    public override float GetPossessionCooldownTimerMax()
    {
        throw new System.NotImplementedException();
    }

    protected override bool IsAlive()
    {
        throw new System.NotImplementedException();
    }

    public void MakeChanges(BaseState currentState)
    {
        ApplyChanges(currentState);
    }

    protected virtual void ApplyChanges(BaseState currentState)
    {

    }
}
