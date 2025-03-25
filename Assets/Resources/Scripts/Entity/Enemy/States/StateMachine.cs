using Unity.VisualScripting;
using UnityEngine;
public class StateMachine : MonoBehaviour
{
    public BaseState activeState;

    public void Initialise()
    {
        ChangeState(new IdleState());
        activeState.enemy.OnEnemyHealthChanged += HealthChanged;
        PossessionManager.Instance.OnPossessed += ChangeState;
    }

    private void ChangeState(object sender, GameObject possessedObject)
    {
        var Possessable = possessedObject.GetComponent<IPossessable>();
        if (Possessable != null && Possessable == activeState.enemy.GetComponent<IPossessable>())
        {
            ChangeState(new PossessedState());
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

    private void HealthChanged(object sender, float health)
    {
        if (health > 30 && health < 50)
        {
            ChangeState(new FleeState());
        }

        else if (health < 30)
        {
            ChangeState(new HealState());
        }
    }
}


