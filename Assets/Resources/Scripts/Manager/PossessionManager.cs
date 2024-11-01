using UnityEngine;

public sealed class PossessionManager : MonoBehaviour
{
    public static IPossessible currentlyPossessed = null;
    public static Entity entity;

    public static IPossessible ToPossess(IPossessible possessible, Entity e)
    {
        if (possessible != null && currentlyPossessed != possessible)
        {
            currentlyPossessed = possessible;
            entity = e;
            return currentlyPossessed;
        }

        return null;
    }

    public static void UnPossessEntity()
    {
        if (currentlyPossessed != null)
        {
            currentlyPossessed = null;
        }

    }

}
