using System;
using UnityEngine;

public class PossessionManager : IManagable
{

    private static PossessionManager Instance;
    public static PossessionManager instance { get { return Instance == null ? Instance = new PossessionManager() : Instance; } }

    public event EventHandler<IPossessable> OnPossessed;

    private Possession currentPossession;
    private IPossessable currentPossessable;

    private PlayerController playerController;

    public void Initialize()
    {

    }

    public void PostInitialize()
    {
        playerController = PlayerManager.instance.GetPlayer();
        ToPossess(playerController.gameObject);
    }

    public Possession ToPossess(GameObject possessable)
    {
        if (possessable)
        {
            if (currentPossessable != null)
                ToDepossess(currentPossessable.GetPossessedEntity().gameObject);

            currentPossessable = possessable.GetComponent<IPossessable>();
            currentPossessable.Possessing(possessable);

            currentPossession = new Possession(currentPossessable);
            OnPossessed?.Invoke(this, currentPossessable);
            return currentPossession;
        }

        return null;
    }

    public void ToDepossess(GameObject possessable)
    {
        currentPossessable = possessable.GetComponent<IPossessable>();
        currentPossessable.Depossessing(possessable);

        if (currentPossessable != null)
        {
            currentPossessable = null;
            currentPossession = null;
        }
    }

    public Possession GetCurrentPossession() => currentPossession;

    public IPossessable GetCurrentPossessable() => currentPossessable;

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
