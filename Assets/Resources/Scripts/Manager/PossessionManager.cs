public static class PossessionManager
{
    public static IPossessible currentlyPossessed = null;
    public static Entity entity;

    public static IPossessible ToPossess(IPossessible possessible)
    {
        
        if (possessible != null && currentlyPossessed != possessible)
        {
            currentlyPossessed = possessible;
            return currentlyPossessed;
        }

        return null;
    }

    public static void ToDepossess()
    {
        if (currentlyPossessed != null)
        {
            currentlyPossessed = null;
        }

    }
}
