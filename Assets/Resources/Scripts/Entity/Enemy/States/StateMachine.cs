using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public BaseState activeState;
    public BaseState lastActiveState;

    private Enemy enemy;
    private Dictionary<Type, BaseState> availableStates;

    [Header("State Machine")]
    [SerializeField] private string currentState;

    public void Initialise<T>(T type, Dictionary<Type, BaseState> _availableStates) where T : Entity
    {
        availableStates = _availableStates;

        if (type is Enemy)
        {
            enemy = type.GetComponent<Enemy>();
            enemy.OnDamaged += Enemy_OnDamaged;
            ChangeState(new IdleState(enemy));
        }
    }

    private void Enemy_OnDamaged(object sender, IDamageable.OnDamagedEventArgs e)
    {
        if (enemy.GetHealth() > 30 && enemy.GetHealth() < 50)
        {
            ChangeState(new FleeState(enemy));
        }

        else if (enemy.GetHealth() < 30)
        {
            ChangeState(new HealState(enemy));
        }
    }

    private void Update()
    {
        if (activeState != null)
        {
            activeState.Perform();
        }
    }

    public void ChangeState(BaseState newState)
    {
        if (activeState != null && activeState.GetType() == newState.GetType())
        {
            return;
        }

        if (activeState != null)
        {
            if (activeState != new PossessedState(enemy))
            {
                lastActiveState = activeState;
            }
            else
            {
                lastActiveState = null;
            }

            activeState.Exit();
        }

        activeState = newState;

        if (activeState != null)
        {
            activeState.stateMachine = this;
            activeState.Enter();
            currentState = activeState?.ToString() ?? "None";
        }
    }
}


