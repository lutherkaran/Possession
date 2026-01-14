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

    protected StateMachine animalStateMachine;
    private NavMeshAgent animalAgent;

    protected Dictionary<Type, BaseState> animalStates;

    public override void Initialize()
    {
        base.Initialize();

        animalAgent = GetComponent<NavMeshAgent>();
        animalStateMachine = GetComponent<StateMachine>();
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
