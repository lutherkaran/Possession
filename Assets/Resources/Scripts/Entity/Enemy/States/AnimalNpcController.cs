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
            animal.GetNavMeshAgent().isStopped = false;
        }

        else if (stateSettings.currentActiveState is FleeState)
        { 
            animal.GetNavMeshAgent().velocity *= 2f;
            animal.GetNavMeshAgent().isStopped = false;
        }

        else if (stateSettings.currentActiveState is PossessedState)
        {
            animal.GetAnimalAnimator().SetFloat("Blending", animal.GetActualSpeed());
            animal.GetNavMeshAgent().isStopped = true;
        }
    }

    public void Reset()
    {
        if (stateSettings.currentActiveState is IdleState)
        {
            //animal.GetAnimalAnimator().SetFloat("IsWalking", animal.GetNavMeshAgent().velocity.magnitude); // idle true
            animal.GetNavMeshAgent().isStopped = false;
        }
        else if (stateSettings.currentActiveState is PatrolState)
        {
            //animal.GetAnimalAnimator().SetFloat("IsWalking", animal.GetNavMeshAgent().velocity.magnitude);
            animal.GetNavMeshAgent().isStopped = false;
        }
        else if (stateSettings.currentActiveState is FleeState)
        {
            //animal.GetAnimalAnimator().SetFloat("IsWalking", animal.GetNavMeshAgent().velocity.magnitude);
            //animal.GetNavMeshAgent().velocity = Vector3.zero;
            animal.GetNavMeshAgent().isStopped = false;
        }

        else if (stateSettings.currentActiveState is PossessedState)
        {
            //animal.GetAnimalAnimator().SetFloat("IsWalking", animal.GetNavMeshAgent().velocity.magnitude);
            //animal.GetNavMeshAgent().velocity = UnityEngine.Vector3.zero;
            animal.GetNavMeshAgent().isStopped = false;
        }
    }
}
