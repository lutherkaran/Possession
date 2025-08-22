using Unity.VisualScripting;
using UnityEngine;
public class StateMachine : MonoBehaviour
{
    public BaseState activeState;
    private Enemy enemy;

    public void Initialise<T>(T type) where T : Entity
    {
        if (type is Enemy)
        {
            enemy = type.GetComponent<Enemy>();
            enemy.OnEnemyHealthChanged += HealthChanged;
            ChangeState(new IdleState(enemy));
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
            //activeState.enemy = GetComponent<Enemy>();
            activeState.Enter();
        }
    }

    private void HealthChanged(object sender, float health)
    {
        if (health > 30 && health < 50)
        {
            ChangeState(new FleeState(enemy));
        }

        else if (health < 30)
        {
            ChangeState(new HealState(enemy));
        }
    }
}


