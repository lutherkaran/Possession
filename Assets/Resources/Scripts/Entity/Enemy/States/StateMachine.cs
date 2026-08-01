using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public BaseState currentActiveState;
    public BaseState lastActiveState;

    private IStateContext stateContext;

    private Dictionary<Type, BaseState> availableStates;

    private float waitTimer = 0;

    [Header("State Machine")]
    [SerializeField] private string currentState;

    public void  Initialise(IStateContext _stateContext, Dictionary<Type, BaseState> _availableStates)
    {
        availableStates = _availableStates;

        if (_stateContext == null)
        {
            Debug.LogError("Statemachine requires a component implementation" + stateContext);
        }
        else
        {
            stateContext = _stateContext;
            ChangeState(new IdleState(stateContext));
        }
    }
    public void Refresh(float deltaTime)
    {
        if (currentActiveState != null)
        {
            currentActiveState.Perform();
        }
    }

    public void ChangeState(BaseState newState)
    {
        if (currentActiveState != null && currentActiveState.GetType() == newState.GetType())
        {
            return;
        }

        if (currentActiveState != null)
        {
            if (currentActiveState != new PossessedState(stateContext))
            {
                lastActiveState = currentActiveState;
            }
            else
            {
                lastActiveState = null;
            }

            currentActiveState.Exit();
        }

        currentActiveState = newState;

        if (currentActiveState != null)
        {
            currentActiveState.stateMachine = this;
            currentActiveState.Enter();
            currentState = currentActiveState?.ToString() ?? "None";
        }
    }

    public void Waiting(BaseState newState, float duration)
    {
        waitTimer += Time.deltaTime;

        if (waitTimer > duration)
        {
            ChangeState(newState);
            waitTimer = 0;
        }
    }
}


