using UnityEngine;

public class StateSettings
{
    public IStateContext stateContext;
    public BaseState currentActiveState;

    public bool isAlert { get; set; } = false;
    public bool isIdle { get; set; } = false;

    public float fieldOfView { get; set; }

    public bool isPatrolling { get; set; } = false;
    public Vector3 desiredVelocity = Vector3.zero;

    public StateSettings(IStateContext _stateContext, BaseState _baseState, bool _isAlert, bool _isIdle, bool _isPatrolling, Vector3 _desiredVelocity, float _fieldOfView)
    {
        stateContext = _stateContext;
        currentActiveState = _baseState;
        isAlert = _isAlert;
        isIdle = _isIdle;
        isPatrolling = _isPatrolling;
        desiredVelocity = _desiredVelocity;
        fieldOfView = fieldOfView;
    }
}
