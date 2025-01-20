using UnityEngine;
public class StateMachine : MonoBehaviour
{
    public BaseState activeState;

    public void Initialise()
    {
        ChangeState(new IdleState());
        activeState.enemy.OnEnemyHealthChanged += HealthChanged;
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
            activeState.Exit();
        }

        activeState = newState;

        if (activeState != null)
        {
            activeState.stateMachine = this;
            activeState.enemy = GetComponent<Enemy>();
            activeState.Enter();
        }
    }

    private void HealthChanged(object sender, float e)
    {
        if (e > 30 && e < 50)
        {
            ChangeState(new FleeState());
        }

        else if (e < 30)
        {
            ChangeState(new HealState());
        }
    }
}


