using System;
using UnityEngine;

public class PossessionManager : IManagable
{
    private static PossessionManager Instance;
    public static PossessionManager instance { get { return Instance == null ? Instance = new PossessionManager() : Instance; } }

    public event EventHandler<IPossessable> OnPossessed;

    private Possession currentPossession;
    private IPossessable currentlyPossessed;

    private PlayerController playerController;
    private TargetLocker targetLocker;

    private bool isFirstPossession;

    public void Initialize()
    {
        isFirstPossession = true;
    }

    public void PostInitialize()
    {
        playerController = PlayerManager.instance.GetPlayer();
        targetLocker = playerController.GetComponent<TargetLocker>();

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
        if (!isFirstPossession)
            ToDepossess(currentlyPossessed.GetPossessedEntity().gameObject);

        currentlyPossessed = possessable;
        currentlyPossessed.Possessing(currentlyPossessed.GetPossessedEntity().gameObject);

        currentPossession = new Possession(currentlyPossessed, targetLocker);
        OnPossessed?.Invoke(this, currentlyPossessed);

        return currentPossession;
    }

    public void ToDepossess(GameObject possessable)
    {
        currentlyPossessed.Depossessing(possessable);

        currentlyPossessed = null;
        currentPossession = null;
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
