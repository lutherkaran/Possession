using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public BaseState activeState;
    public BaseState possessedState;

    public void Initialise()
    {
        ChangeState(new IdleState());
    }

    public void Update()
    {
        // if enemy isn't possessed by the player then perform actions else stay idle.
        
        if (activeState != null && PossessionManager.Instance.currentlyPossessed != activeState.enemy.PlayerPossessed)
        {
            activeState.Perform();
        }

        else
        {
            ChangeState(new PossessedState());
            possessedState = activeState;
            activeState.Perform();
        }
    }

    public void ChangeState(BaseState newState)
    {
        if (activeState != null)
        {
            activeState.Exit();
        }

        activeState = newState;

        if (activeState != null)
        {
            activeState.stateMachine = this;
            activeState.enemy = GetComponent<Enemy>();
            if (PossessionManager.Instance.currentlyPossessed != activeState.enemy.PlayerPossessed)
                activeState.Enter();
        }
    }
}
// I wanna have a behaviour in which when I possess the enemy the state machine stops.
// Should I create possessible state?

