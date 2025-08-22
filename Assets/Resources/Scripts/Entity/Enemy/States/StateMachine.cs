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
}


