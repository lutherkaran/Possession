using UnityEngine;
using UnityEngine.AI;

public class AnimalNpcController
{
    private readonly AnimalNpc animal;

    public AnimalNpcController(AnimalNpc animal)
    {
        this.animal = animal;
    }

    // Reads state identity from the StateMachine directly -- see EnemyAnimationController for
    // why this is no longer read off StateSettings.
    public void RunAI(StateSettings stateSettings)
    {
        var activeState = animal.GetStateMachine().currentActiveState;

        if (activeState is IdleState)
        {
            animal.GetNavMeshAgent().velocity = Vector3.zero;
            animal.GetNavMeshAgent().isStopped = true;
        }
        else if (activeState is PatrolState)
        {
            animal.GetNavMeshAgent().SetDestination(animal.FindTargetLocation());
            animal.GetNavMeshAgent().velocity = default;
            animal.GetNavMeshAgent().isStopped = false;
        }
        else if (activeState is FleeState)
        {
            animal.GetNavMeshAgent().speed *= 2f;
            animal.GetNavMeshAgent().isStopped = false;
        }
        else if (activeState is PossessedState)
        {
            animal.GetNavMeshAgent().isStopped = true;
            animal.GetNavMeshAgent().enabled = false;
            animal.GetNavMeshAgent().velocity = Vector3.zero;

            if (animal.GetRigidBody() != null)
                animal.GetRigidBody().isKinematic = false;
        }
    }

    public void Reset()
    {
        var activeState = animal.GetStateMachine().currentActiveState;

        if (activeState is IdleState)
        {
            animal.GetNavMeshAgent().isStopped = false;
        }
        else if (activeState is PatrolState)
        {
            animal.GetNavMeshAgent().isStopped = false;
        }
        else if (activeState is FleeState)
        {
            animal.GetNavMeshAgent().speed /= 2f;
            animal.GetNavMeshAgent().isStopped = false;
        }
        else if (activeState is PossessedState)
        {
            if (animal.GetRigidBody() != null)
                animal.GetRigidBody().isKinematic = true;

            var agent = animal.GetNavMeshAgent();
            agent.enabled = true;

            if (!agent.isOnNavMesh)
            {
                if (NavMesh.SamplePosition(animal.transform.position, out NavMeshHit hit, 10f, NavMesh.AllAreas))
                {
                    agent.Warp(hit.position);
                }
                else
                {
                    Debug.LogWarning($"{animal.name}: could not find a NavMesh within 10m after depossession -- agent may stay disabled-feeling until it does.");
                }
            }

            agent.isStopped = false;
            animal.GetStateMachine().ChangeState(animal.GetStateMachine().lastActiveState);
        }
    }
}