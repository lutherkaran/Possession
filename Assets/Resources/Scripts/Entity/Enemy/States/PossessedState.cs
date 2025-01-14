using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossessedState : BaseState
{
    public override void Enter()
    {
        // play Possessed animation once..
        enemy.Agent.velocity = Vector3.zero;
    }

    public override void Perform()
    {
        //Debug.Log("I'm Possessed");
    }

    public override void Exit()
    {
        //stateMachine.ChangeState(new IdleState());
    }

}
