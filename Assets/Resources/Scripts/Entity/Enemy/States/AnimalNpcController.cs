using UnityEngine;

public class AnimalNpcController
{
    private AnimalNpc animal;
    private StateSettings stateSettings;

    public AnimalNpcController(AnimalNpc _animal)
    {
        animal = _animal;
    }

    public void RunAI(StateSettings _stateSettings)
    {
        stateSettings = _stateSettings;

        if (stateSettings.currentActiveState is IdleState)
        {
            animal.GetNavMeshAgent().velocity = Vector3.zero;
            animal.GetNavMeshAgent().isStopped = true;
        }

        else if (stateSettings.currentActiveState is PatrolState)
        {
            animal.GetNavMeshAgent().SetDestination(animal.FindTargetLocation());
            animal.GetNavMeshAgent().velocity = default;
            animal.GetNavMeshAgent().isStopped = false;
        }

        else if (stateSettings.currentActiveState is FleeState)
        {
            animal.GetNavMeshAgent().speed *= 2f;
            animal.GetNavMeshAgent().isStopped = false;
        }

        else if (stateSettings.currentActiveState is PossessedState)
        {
            animal.GetNavMeshAgent().isStopped = true;
            animal.GetNavMeshAgent().enabled = false;
            animal.GetNavMeshAgent().velocity = Vector3.zero;

            // The Rigidbody stays kinematic while the NavMeshAgent drives movement (so it
            // doesn't fight the agent's direct position writes). Only while actually possessed
            // and player-driven via MovePosition does it need to be a real dynamic body so
            // wall/obstacle collisions are respected.
            if (animal.GetRigidBody() != null)
                animal.GetRigidBody().isKinematic = false;
        }
    }

    public void Reset()
    {
        if (stateSettings.currentActiveState is IdleState)
        {
            animal.GetNavMeshAgent().isStopped = false;
        }
        else if (stateSettings.currentActiveState is PatrolState)
        {
            animal.GetNavMeshAgent().isStopped = false;
        }
        else if (stateSettings.currentActiveState is FleeState)
        {
            // Was never undoing the *2 speed boost applied in RunAI -- every flee would
            // permanently double the animal's speed again (compounding: 2x, 4x, 8x...).
            animal.GetNavMeshAgent().speed /= 2f;
            animal.GetNavMeshAgent().isStopped = false;
        }

        else if (stateSettings.currentActiveState is PossessedState)
        {
            animal.GetNavMeshAgent().enabled = true;
            animal.GetNavMeshAgent().isStopped = false;

            // Back to kinematic before handing control back to the NavMeshAgent.
            if (animal.GetRigidBody() != null)
                animal.GetRigidBody().isKinematic = true;

            animal.GetStateMachine().ChangeState(animal.GetStateMachine().lastActiveState);
        }
    }
}