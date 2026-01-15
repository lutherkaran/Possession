using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AnimalNpc : Npc
{
    public enum animalType { Cat, Dog, Chicken, Tiger, Penguin, Horse, Deer }
    public animalType animal;

    IPuzzleObject puzzleObject;

    [SerializeField] private Gem gem;
    [SerializeField] protected Animator animalAnimator;

    private NavMeshAgent animalAgent;

    public override void Initialize()
    {
        base.Initialize();

        animalAgent = GetComponent<NavMeshAgent>();
    }

    protected override void ApplyChanges(BaseState a)
    {
        base.ApplyChanges(a);

        if (a is IdleState)
        {
            GetAnimal().GetAnimalAnimator().SetBool("IsWalking", false); // idle true
            GetAnimalNpcAgent().velocity = Vector3.zero;

            GetAnimal().GetAnimalNpcAgent().isStopped = true;

        }
        else if (a is PatrolState)
        {
            GetAnimal().GetComponent<Chicken>().MoveToLocation(Time.fixedDeltaTime);
            GetAnimal().GetAnimalAnimator().SetBool("IsWalking", true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        puzzleObject = other.transform.GetComponent<IPuzzleObject>();

        if (DoorPuzzle.puzzleDictionary[puzzleObject] == gem.GetGemSO().animal)
        {
            puzzleObject.Collected();
        }
        else
        {
            Debug.Log("Find another Gem");
        }
    }

    public NavMeshAgent GetAnimalNpcAgent() => animalAgent;

    public abstract Animator GetAnimalAnimator();
    public abstract AnimalNpc GetAnimal();
}
