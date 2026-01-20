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

    [SerializeField] private float maxDistance = 5f;

    protected NavMeshAgent animalAgent;
    protected Dictionary<Type, BaseState> animalStates;
    protected StateMachine animalStateMachine;

    protected AnimalNpcController animalNpcController;

    [SerializeField] private Transform[] pathPoints;

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
            { typeof(PossessedState), new PossessedState(this) },
            { typeof(FleeState), new FleeState(this) }
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

    public NavMeshAgent GetNavMeshAgent()
    {
        return animalAgent;
    }

    public override void Possessing(GameObject go)
    {
        base.Possessing(go);

        possessedByPlayer = PossessionManager.instance.GetCurrentPossessable();
        animalStateMachine.ChangeState(new PossessedState(this));
    }

    public override void Depossessing(GameObject go)
    {
        base.Depossessing(go);

        possessedByPlayer = null;
        animalStateMachine.ChangeState(new IdleState(this));
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

    public bool IsSafe()
    {
        if (Vector3.Distance(transform.position, PlayerManager.instance.GetPlayer().transform.position) >= maxDistance)
            return true;
        else
            return false;
    }

    public Vector3 FindTargetLocation()
    {
        int randomIndex = UnityEngine.Random.Range(0, pathPoints.Length);
        Vector3 targetLocation = pathPoints[randomIndex].position;
        return targetLocation;
    }
    
    public virtual void ApplySettings(StateSettings _settings)
    {
        
    }

    public void ResetChanges()
    {
        animalNpcController.Reset();
    }

    bool IStateContext.CanSeePlayer() => false;

    public Transform GetTransform() => transform;

    public abstract Animator GetAnimalAnimator();
    public abstract AnimalNpc GetAnimal();
}
