public class PossessionManager
{
    private static PossessionManager instance;
    public static PossessionManager Instance { get { return instance == null ? instance = new PossessionManager() : instance; } }
    public IPossessible currentlyPossessed = null;

    public IPossessible ToPossess(IPossessible possessible)
    {
        
        if (possessible != null && currentlyPossessed != possessible)
        {
            currentlyPossessed = possessible;
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
