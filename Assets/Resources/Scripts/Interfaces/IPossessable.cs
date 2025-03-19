using UnityEngine;

public interface IPossessable
{
    void Possessing(GameObject go);
    void Depossessing(GameObject go);
    Entity GetEntity();
}
