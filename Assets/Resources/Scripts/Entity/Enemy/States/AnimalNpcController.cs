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
            animal.GetNavMeshAgent().velocity *= 2f;
            animal.GetNavMeshAgent().isStopped = false;
        }

        else if (stateSettings.currentActiveState is PossessedState)
        {
            animal.GetNavMeshAgent().isStopped = true;
            animal.GetNavMeshAgent().enabled = false;
            animal.GetNavMeshAgent().velocity = Vector3.zero;
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
            animal.GetNavMeshAgent().isStopped = false;
        }

        else if (stateSettings.currentActiveState is PossessedState)
        {
            animal.GetNavMeshAgent().enabled = true;
            animal.GetNavMeshAgent().isStopped = false;
            animal.GetStateMachine().ChangeState(animal.GetStateMachine().lastActiveState);
        }
    }
}
