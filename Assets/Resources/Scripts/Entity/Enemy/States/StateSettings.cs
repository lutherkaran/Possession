using UnityEngine;

public class StateSettings
{
    private float fieldOfView;
    public Vector3 desiredVelocity { get; private set; }

    public enum animationStates
    {
        isIdle = 0,
        isWalking = 1,
        isRunning = 2,
        isAttacking = 3,
        isPossessed = 4,
    }

    public animationStates animStates;

    public StateSettings(animationStates animStates, Vector3 desiredVelocity, float fieldOfView)
    {
        this.animStates = animStates;
        this.desiredVelocity = desiredVelocity;
        this.fieldOfView = fieldOfView;
    }

    public void UpdateSettings(animationStates _animStates, Vector3 _desiredVelocity, float _fieldOfView)
    {
        animStates = _animStates;
        desiredVelocity = _desiredVelocity;
        fieldOfView = _fieldOfView;
    }
}