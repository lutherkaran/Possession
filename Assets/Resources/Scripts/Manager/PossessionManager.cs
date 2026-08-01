using System;
using UnityEngine;

public class PossessionManager : IManagable
{
    private static PossessionManager Instance;
    public static PossessionManager instance { get { return Instance == null ? Instance = new PossessionManager() : Instance; } }

    public event EventHandler<IPossessable> OnPossessed;

    private Possession currentPossession;
    private PlayerController playerController;

    private IPossessable currentlyPossessed;

    public bool isFirstPossession { get; set; }

    public void Initialize()
    {
        isFirstPossession = true;
    }

    public void PostInitialize()
    {
        playerController = PlayerManager.instance.GetPlayer();

        currentlyPossessed = playerController;
        ToPossess(playerController);
        isFirstPossession = false;
    }

    public Possession ToPossess(GameObject possessable)
    {
        var targetToPossess = possessable.GetComponent<IPossessable>();
        return ToPossess(targetToPossess);
    }

    public Possession ToPossess(IPossessable possessable)
    {
        if (!isFirstPossession & currentlyPossessed != null)
            ToDepossess(currentlyPossessed);

        currentlyPossessed = possessable;
        var toPossess = currentlyPossessed.GetPossessedEntity().gameObject;
        currentlyPossessed.Possessing(toPossess);

        currentPossession = new Possession(currentlyPossessed);
        OnPossessed?.Invoke(this, currentlyPossessed);

        return currentPossession;
    }

    public void ToDepossess(IPossessable depossessable)
    {
        var toDepossess = depossessable.GetPossessedEntity().gameObject;
        currentlyPossessed.Depossessing(toDepossess);
        depossessable.GetPossessedEntity().StopPhysicsMovement(); // avoid leftover Rigidbody drift after depossession

        currentlyPossessed = null;
        currentPossession = null;
    }

    public Transform GetActiveTransform()
    {
        return currentlyPossessed?.GetPossessedEntity().transform;
    }

    public Possession GetCurrentPossession() => currentPossession;

    public IPossessable GetCurrentPossessable() => currentlyPossessed;

    public void Refresh(float deltaTime)
    {

    }

    public void PhysicsRefresh(float fixedDeltaTime)
    {

    }

    public void LateRefresh(float deltaTime)
    {

    }

    public void OnDemolish()
    {
        Instance = null;
    }
}