using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    protected GameObject gameObject;
    protected EnemyAnimator animator;
    protected Dictionary<Type, BaseState> availableStates;

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

    public StateMachine stateMachine;

    protected virtual void EnterState() { }
    protected virtual void PerformState() { }
    protected virtual void ExitState() { }

    public void Enter() => EnterState();
    public void Perform() => PerformState();
    public void Exit() => ExitState();

    public GameObject GetGameObject() { return gameObject; }
    public EnemyAnimator GetAnimator() { return animator; }
    public Dictionary<Type, BaseState> GetAvailableStates() { return availableStates; }
}
