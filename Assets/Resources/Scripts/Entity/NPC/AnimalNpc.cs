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

    [SerializeField] protected float safeDistance = 10f;
    [SerializeField] protected EntityAnimation animalAnimation;

    protected NavMeshAgent animalAgent;
    protected Dictionary<Enum, BaseState> animalStates;
    protected StateMachine animalStateMachine;

    protected AnimalNpcController animalNpcController;

    [SerializeField] private Transform[] pathPoints;

    private bool isCaged = false;
    private readonly float cageDuration = 3f;

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
        animalStates = new Dictionary<Enum, BaseState>()
        {
            { BaseState.StateType.Idle, new IdleState(this) },
            { BaseState.StateType.Patrol, new PatrolState(this) },
            { BaseState.StateType.Possessed, new PossessedState(this) },
            { BaseState.StateType.Flee, new FleeState(this) },
        };

        animalStateMachine.Initialise(this, animalStates);
    }

    public override void Refresh(float deltaTime)
    {
        animalStateMachine.Refresh(deltaTime);

        float actualSpeed = GetNavMeshAgent().velocity.magnitude / GetNavMeshAgent().speed;

        if (PossessionManager.instance.GetCurrentPossessable() != possessedByPlayer)
            animalAnimation.SetSpeed(actualSpeed);

    }

    public override void PhysicsRefresh(float fixedDeltaTime)
    {
        currentFixedDeltaTime = fixedDeltaTime;
    }

    public NavMeshAgent GetNavMeshAgent()
    {
        return animalAgent;
    }

    public StateMachine GetStateMachine() => animalStateMachine;

    public override void Possessing(GameObject go)
    {
        base.Possessing(go);

        possessedByPlayer = PossessionManager.instance.GetCurrentPossessable();
        animalStateMachine.ChangeState(new PossessedState(this));
    }

    public override void Depossessing(GameObject go)
    {
        base.Depossessing(go);
        animalStateMachine.ChangeState(animalStateMachine.lastActiveState ?? new IdleState(this));
        possessedByPlayer = null;
    }

    // Called by SuspicionState when a guard captures this animal while it's possessed.
    // Low-stakes consequence: the animal pauses ('caged') for a few seconds, then returns to
    // patrolling. Swap for a real cage animation/anchor point per level as art needs it.
    public void OnCaptured()
    {
        if (isCaged) return;
        isCaged = true;

        if (animalAgent != null) animalAgent.enabled = false;
        animalStateMachine.ChangeState(new IdleState(this));

        Invoke(nameof(ReleaseFromCage), cageDuration);
    }

    private void ReleaseFromCage()
    {
        isCaged = false;
        if (animalAgent != null) animalAgent.enabled = true;
        animalStateMachine.ChangeState(new PatrolState(this));
    }

    private void OnTriggerEnter(Collider other)
    {
        puzzleObject = other.transform.GetComponent<IPuzzleObject>();
        if (puzzleObject == null) return; // not every trigger collider in the level is a gem

        if (!DoorPuzzle.puzzleDictionary.TryGetValue(puzzleObject, out animalType requiredAnimal))
            return; // unregistered/stale puzzle object -- avoid the KeyNotFoundException crash

        if (requiredAnimal == gem.GetGemSO().animal)
        {
            puzzleObject.Collected();
        }
        else
        {
            Debug.Log("Find another Gem");
        }
    }

    public virtual bool IsSafe()
    {
        return true;
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

    bool IStateContext.CanSeePossessedPlayer() => false;

    public Transform GetTransform() => transform;

    public abstract Animator GetAnimalAnimator();
    public abstract AnimalNpc GetAnimal();

    public EntityAnimation GetAnimationEntity()
    {
        return entityAnimation;
    }

    public bool CanSeePossessedAnimal()
    {
        return false;
    }
}