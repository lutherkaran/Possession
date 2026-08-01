using UnityEngine;
using UnityEngine.AI;

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
            animal.GetNavMeshAgent().speed /= 2f;
            animal.GetNavMeshAgent().isStopped = false;
        }

        else if (stateSettings.currentActiveState is PossessedState)
        {
            if (animal.GetRigidBody() != null)
                animal.GetRigidBody().isKinematic = true;

            var agent = animal.GetNavMeshAgent();
            agent.enabled = true;

            // While possessed, Rigidbody-driven movement isn't constrained to the baked
            // NavMesh, so the animal can end up standing somewhere the mesh doesn't cover.
            // Re-enabling the agent doesn't automatically reattach it to the mesh in that case
            // (isOnNavMesh stays false and every subsequent agent call throws) -- Warp finds
            // the nearest valid point and properly re-registers the agent on it.
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