using System;
using UnityEngine;

public class PossessionManager
{
    public event EventHandler<IPossessable> OnPossessed;

    private static PossessionManager instance;
    public static PossessionManager Instance { get { return instance == null ? instance = new PossessionManager() : instance; } }
   
    private Possession currentPossession;
    private IPossessable currentPossessable;

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
}
