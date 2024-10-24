using UnityEngine;

public sealed class PossessionManager : MonoBehaviour
{
    public static IPossessible currentlyPossessed = null;
    public static IPossessible Possessing(IPossessible possessible)
    {
        if (possessible != null && currentlyPossessed != possessible)
        {
            currentlyPossessed = possessible;
            return currentlyPossessed;
        }
        return null;
    }

    public static void UnPossessing()
    {
        if (currentlyPossessed != null)
        {
            currentlyPossessed = null;
        }

    }

}
