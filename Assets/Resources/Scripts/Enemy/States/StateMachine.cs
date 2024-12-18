using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public BaseState activeState;

    public void Initialise()
    {
        ChangeState(new IdleState());
    }

    public void Update()
    {
        // If enemy health is < 50 change activestate to flee state.

        if (activeState != null && PossessionManager.currentlyPossessed != activeState.enemy.PlayerPossessed)
        {
            activeState.Perform();
        }
        else
        {
            activeState.enemy.Agent.velocity = Vector3.zero;
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
            if (PossessionManager.currentlyPossessed != activeState.enemy.PlayerPossessed)
                activeState.Enter();
        }
    }
}
// I wanna have a behaviour in which when I possess the enemy the state machine stops.
// Should I create possessible state?

