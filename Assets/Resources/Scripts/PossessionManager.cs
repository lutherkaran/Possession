using UnityEngine;

public sealed class PossessionManager : MonoBehaviour
{
    public static IPossessible currentlyPossessed;
    public static GameObject go;

    public static IPossessible Possessing(IPossessible possessible, GameObject g)
    {
        go = g;
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
            go = null;
        }

    }
}
