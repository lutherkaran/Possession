using System;

public class PossessionManager
{
    private static PossessionManager instance;
    private Possession currentPossession;

    public static PossessionManager Instance { get { return instance == null ? instance = new PossessionManager() : instance; } }
    public IPossessable currentlyPossessed = null;

    public event EventHandler<IPossessable> OnPossessed;

    public Possession ToPossess(IPossessable possessible)
    {
        if (possessible != null && currentlyPossessed != possessible)
        {
            currentlyPossessed = possessible;
            currentPossession = new Possession(currentlyPossessed);
            OnPossessed?.Invoke(this, currentlyPossessed);
            return currentPossession;
        }

        return null;
    }

    public void ToDepossess(IPossessable possessable)
    {
        currentlyPossessed = possessable;
        if (currentlyPossessed != null)
        {
            currentlyPossessed = null;
            currentPossession = null;
        }

    }
}
