using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AnimalNpc : Npc, IStateContext
{
    public enum animalType { Cat, Dog, Chicken, Tiger, Penguin, Horse, Deer }
    public animalType animal;

    IPuzzleObject puzzleObject;

    [SerializeField] private Gem gem;
    [SerializeField] protected Animator animalAnimator;

    protected NavMeshAgent animalAgent;
    protected Dictionary<Type, BaseState> animalStates;
    protected StateMachine animalStateMachine;

    private AnimalNpcController animalNpcController;

    public override void Initialize()
    {
        base.Initialize();

        animalAgent = GetComponent<NavMeshAgent>();
        animalStateMachine = GetComponent<StateMachine>();
        animalNpcController = new AnimalNpcController(this);
    }

    public override void PostInitialize()
    {
        InitializeAnimalStateDictionary();
    }

    private void InitializeAnimalStateDictionary()
    {
        animalStates = new Dictionary<Type, BaseState>()
        {
            { typeof(IdleState), new IdleState(this) },
            { typeof(PatrolState), new PatrolState(this) },
            { typeof(PossessedState), new PossessedState(this) }
        };

        animalStateMachine.Initialise(this, animalStates);
    }


    public override void Refresh(float deltaTime)
    {
        animalStateMachine.Refresh(deltaTime);
    }

    public override void PhysicsRefresh(float fixedDeltaTime)
    {
        currentFixedDeltaTime = fixedDeltaTime;
    }

    public abstract Animator GetAnimalAnimator();
    public abstract AnimalNpc GetAnimal();

    public NavMeshAgent GetNavMeshAgent()
    {
        return animalAgent;
    }

    public new void Possessing(GameObject go)
    {
        possessedByPlayer = PossessionManager.instance.GetCurrentPossessable();
        animalStateMachine.ChangeState(new PossessedState(this));
    }

    public new void Depossessing(GameObject go)
    {
        animalStateMachine.ChangeState(new IdleState(this));
        possessedByPlayer = null;
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

    public bool CanSeePlayer()
    {
        return false;
    }

    public void MakeChanges(StateSettings _settings)
    {
        animalNpcController.RunAI(_settings);
    }

    public void ResetChanges()
    {
        GetAnimal().GetAnimalAnimator().SetBool("IsWalking", true);
        GetAnimal().GetNavMeshAgent().velocity = Vector3.one;
        GetAnimal().GetNavMeshAgent().isStopped = false;
    }
}
