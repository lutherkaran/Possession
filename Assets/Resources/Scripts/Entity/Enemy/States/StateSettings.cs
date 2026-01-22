using UnityEngine;

public class StateSettings
{
    public IStateContext stateContext;

    public BaseState currentActiveState;

    public float fieldOfView { get; set; }

    public Vector3 desiredVelocity = Vector3.zero;

    public enum animationStates
    {
        isIdle = 0,
        isWalking = 1,
        isRunning = 2,
        isAttacking = 3,
        isPossessed = 4,
    }

    public animationStates animStates;

    public StateSettings(IStateContext _stateContext, BaseState _baseState, animationStates _boolState, Vector3 _desiredVelocity, float _fieldOfView)
    {
        stateContext = _stateContext;
        currentActiveState = _baseState;
        animStates = _boolState;
        desiredVelocity = _desiredVelocity;
        fieldOfView = fieldOfView;
    }
}
