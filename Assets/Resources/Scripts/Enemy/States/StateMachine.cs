using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public BaseState activeState;

    public void Initialise()
    {
        ChangeState(new PatrolState());
    }

    public void Update()
    {
        if (activeState != null && PossessionManager.currentlyPossessed != activeState.enemy.playerPossessed)
        {
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
            if (PossessionManager.currentlyPossessed != activeState.enemy.playerPossessed)
                activeState.Enter();
        }
    }
}
