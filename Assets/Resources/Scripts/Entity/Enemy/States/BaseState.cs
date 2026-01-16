using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    public StateMachine stateMachine;

    public Dictionary<Type, BaseState> GetAvailableStates() { return availableStates; }

    protected GameObject gameObject;
    protected EnemyAnimator animator;
    protected IStateContext stateContext;
    protected Dictionary<Type, BaseState> availableStates;
    
    protected BaseState(IStateContext _stateContext)
    {
        stateContext = _stateContext;
    }

    protected BaseState(GameObject _gameObject)
    {
        gameObject = _gameObject;
    }

    protected BaseState(GameObject _gameObject, EnemyAnimator _animator)
    {
        gameObject = _gameObject;
        animator = _animator;
    }

    protected BaseState(GameObject _gameObject, EnemyAnimator _animator, Dictionary<Type, BaseState> _availableStates)
    {
        gameObject = _gameObject;
        animator = _animator;
        availableStates = _availableStates;
    }

    protected virtual void EnterState() { }
    protected virtual void PerformState() { }
    protected virtual void ExitState() { }

    public void Enter() => EnterState();
    public void Perform() => PerformState();
    public void Exit() => ExitState();

    public GameObject GetGameObject() => gameObject;
    public EnemyAnimator GetAnimator() => animator;
}
