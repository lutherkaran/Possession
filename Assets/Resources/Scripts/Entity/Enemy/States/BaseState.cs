using UnityEngine;

public abstract class BaseState
{
    protected GameObject gameObject;
    protected EnemyAnimator animator;

    protected BaseState(GameObject _gameObject)
    {
        gameObject = _gameObject;
    }

    protected BaseState(GameObject _gameObject, EnemyAnimator _animator)
    {
        gameObject = _gameObject;
        animator = _animator;
    }

    public StateMachine stateMachine;

    public abstract void Enter();
    public abstract void Perform();
    public abstract void Exit();

    public GameObject GetGameObject() { return gameObject; }
}
