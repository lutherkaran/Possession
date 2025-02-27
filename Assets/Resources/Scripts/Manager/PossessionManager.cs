using System;

public class PossessionManager
{
    private static PossessionManager instance;
    public static PossessionManager Instance { get { return instance == null ? instance = new PossessionManager() : instance; } }
    public IPossessable currentlyPossessed = null;

    public event EventHandler<IPossessable> OnPossessed;

    public IPossessable ToPossess(IPossessable possessible)
    {
        if (possessible != null && currentlyPossessed != possessible)
        {
            currentlyPossessed = possessible;
            OnPossessed?.Invoke(this, currentlyPossessed);
            return currentlyPossessed;
        }

        return null;
    }

    public void ToDepossess()
    {
        if (currentlyPossessed != null)
        {
            currentlyPossessed = null;
        }

    }
}
