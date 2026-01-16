using UnityEngine;

public class AnimalNpcController 
{
    private AnimalNpc animal;

    public AnimalNpcController(AnimalNpc _animal)
    {
        animal = _animal;
    }

    public void RunAI(StateSettings _stateSettings)
    {
        if (_stateSettings.currentActiveState is IdleState)
        {
            animal.GetAnimalAnimator().SetBool("IsWalking", false); // idle true
            animal.GetNavMeshAgent().isStopped = true;

            animal.GetNavMeshAgent().velocity = Vector3.zero;

        }
        else if (_stateSettings.currentActiveState is PatrolState)
        {
            animal.GetComponent<Chicken>().MoveToLocation(Time.fixedDeltaTime);
            animal.GetAnimalAnimator().SetBool("IsWalking", true);
        }
    }

    public void Reset()
    {
        animal.GetAnimalAnimator().SetBool("IsWalking", true);
        animal.GetNavMeshAgent().velocity = Vector3.one;
        animal.GetNavMeshAgent().isStopped = false;
    }
}
