using System;
using UnityEngine;

public class PossessionManager
{
    private static PossessionManager instance;
    private Possession currentPossession;
    private IPossessable currentPossessable;

    public static PossessionManager Instance { get { return instance == null ? instance = new PossessionManager() : instance; } }
    public event EventHandler<IPossessable> OnPossessed;

    public Possession ToPossess(GameObject possessable)
    {
        if (possessable)
        {
            currentPossessable = possessable.GetComponent<IPossessable>();
            if (currentPossessable != null)
            {
                currentPossession = new Possession(currentPossessable);
                OnPossessed?.Invoke(this, currentPossessable);
                return currentPossession;
            }
        }

        return null;
    }

    public void ToDepossess(GameObject possessable)
    {
        currentPossessable = possessable.GetComponent<IPossessable>();
        if (currentPossessable != null)
        {
            currentPossessable = null;
            currentPossession = null;
        }

    }

    public Possession GetCurrentPossession() => currentPossession;

    public IPossessable GetCurrentPossessable() => currentPossessable;
}
