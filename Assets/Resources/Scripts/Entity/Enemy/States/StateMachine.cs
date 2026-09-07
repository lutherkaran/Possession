using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public BaseState currentActiveState;
    public BaseState lastActiveState;

    private StateSettings currentStateSettings;
    private IStateContext stateContext;

    private Dictionary<Enum, BaseState> availableStates;

    private float waitTimer = 0;

    [Header("State Machine")]
    [SerializeField] private string currentState;

    public void Initialise(IStateContext stateContext, Dictionary<Enum, BaseState> availableStates)
    {
        this.stateContext = stateContext;
        this.availableStates = availableStates;

        currentStateSettings = new StateSettings(StateSettings.animationStates.isIdle, Vector3.zero, 0f);

        ChangeState(availableStates[BaseState.StateType.Idle]);
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
        if (newState == null) return;

        if (currentActiveState != null && currentActiveState.GetType() == newState.GetType())
        {
            return;
        }

        if (currentActiveState != null)
        {
            lastActiveState = currentActiveState is PossessedState ? null : currentActiveState;
            currentActiveState.Exit();
        }

        currentActiveState = newState;

        currentActiveState.stateMachine = this;
        currentActiveState.Enter();
        currentState = currentActiveState.ToString();
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

    public StateSettings GetCurrentStateSettings() => currentStateSettings;
    public Dictionary<Enum, BaseState> GetAvailableStates() => availableStates;
}