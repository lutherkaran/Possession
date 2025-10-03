using UnityEngine;

public interface IPossessable
{
    void Possessing(GameObject gameObject);
    void Depossessing(GameObject gameObject);
    Entity GetPossessedEntity();
}
