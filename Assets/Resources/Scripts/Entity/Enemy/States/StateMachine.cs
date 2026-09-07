using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public BaseState currentActiveState;
    public BaseState lastActiveState;

    private StateSettings currentStateSettings;
    private IStateContext stateContext;

    private Dictionary<Type, BaseState> availableStates;

    private float waitTimer = 0;

    [Header("State Machine")]
    [SerializeField] private string currentState;

    public void Initialise(IStateContext stateContext, Dictionary<Type, BaseState> availableStates)
    {
        this.stateContext = stateContext;
        this.availableStates = availableStates;

        if (stateContext == null)
        {
            Debug.LogError("Statemachine requires a component implementation" + this.stateContext);
        }
        else
        {
            this.stateContext = stateContext;
            ChangeState(availableStates[typeof(IdleState)]);
            currentStateSettings = new StateSettings(stateContext, currentActiveState, StateSettings.animationStates.isIdle, Vector3.zero, 0);
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
        if (newState == null) return; // guards against callers passing a null lastActiveState, etc.

        if (currentActiveState != null && currentActiveState.GetType() == newState.GetType())
        {
            return;
        }

        if (currentActiveState != null)
        {
            // Was `currentActiveState != new PossessedState(stateContext)` -- comparing against a
            // freshly allocated object is always true by reference, so lastActiveState was never
            // actually protected from being set to a PossessedState. Fixed to a proper type check.
            if (!(currentActiveState is PossessedState))
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

    public StateSettings GetCurrentStateSettings() => currentStateSettings;
    public Dictionary<Type, BaseState> GetAvailableStates() => availableStates;
}