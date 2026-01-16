using UnityEngine;

public class StateSettings
{
    public IStateContext stateContext;
    public BaseState currentActiveState;

    public bool isAlert { get; set; } = false;
    public bool movement { get; set; } = false;

    public float fieldOfView { get; set; }

    public Vector3 desiredVelocity = Vector3.zero;
    public Vector3 targetLocation = Vector3.zero;

    public StateSettings(IStateContext _stateContext, BaseState _baseState, bool _isAlert, bool _movement, Vector3 _targetLocation, Vector3 _desiredVelocity, float _fieldOfView)
    {
        stateContext = _stateContext;
        currentActiveState = _baseState;
        isAlert = _isAlert;
        movement = _movement;
        targetLocation = _targetLocation;
        desiredVelocity = _desiredVelocity;
        fieldOfView = fieldOfView;
    }
}
