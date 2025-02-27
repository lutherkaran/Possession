using UnityEngine;

public interface IPossessable
{
    void Possess(GameObject go);
    void Depossess(GameObject go);
    Entity GetEntity();
}
