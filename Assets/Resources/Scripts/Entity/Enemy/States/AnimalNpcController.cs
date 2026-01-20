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
            animal.GetAnimalAnimator().SetBool("IsWalking", false); // idle true
            animal.GetNavMeshAgent().velocity = Vector3.zero;
            animal.GetNavMeshAgent().isStopped = true;
        }

        else if (stateSettings.currentActiveState is PatrolState)
        {
            animal.GetNavMeshAgent().SetDestination(animal.FindTargetLocation());
            animal.GetAnimalAnimator().SetBool("IsWalking", true);
            animal.GetNavMeshAgent().velocity = Vector3.one * 2f;
            animal.GetNavMeshAgent().isStopped = false;
        }

        else if (stateSettings.currentActiveState is FleeState)
        {
            animal.GetAnimalAnimator().SetBool("IsWalking", true);
            animal.GetNavMeshAgent().velocity = Vector3.one * 4f;
            animal.GetNavMeshAgent().isStopped = false;
        }

        else if (stateSettings.currentActiveState is PossessedState)
        {
            animal.GetAnimalAnimator().SetBool("IsWalking", false);
            animal.GetNavMeshAgent().velocity = UnityEngine.Vector3.zero;
            animal.GetNavMeshAgent().isStopped = true;
        }

    }

    public void Reset()
    {
        if (stateSettings.currentActiveState is IdleState)
        {
            animal.GetAnimalAnimator().SetBool("IsWalking", false); // idle true
            animal.GetNavMeshAgent().isStopped = false;
        }
        else if (stateSettings.currentActiveState is PatrolState)
        {
            animal.GetAnimalAnimator().SetBool("IsWalking", false);
            animal.GetNavMeshAgent().isStopped = false;
        }
        else if (stateSettings.currentActiveState is FleeState)
        {
            animal.GetAnimalAnimator().SetBool("IsWalking", false);
            animal.GetNavMeshAgent().velocity = Vector3.zero;
            animal.GetNavMeshAgent().isStopped = false;
        }

        else if (stateSettings.currentActiveState is PossessedState)
        {
            animal.GetAnimalAnimator().SetBool("IsWalking", false);
            animal.GetNavMeshAgent().velocity = UnityEngine.Vector3.zero;
            animal.GetNavMeshAgent().isStopped = false;
        }
    }
}
