using UnityEngine;

public interface IPossessible
{
    void Possess(GameObject go);
    void Depossess(GameObject go);
    Entity GetEntity();
}
