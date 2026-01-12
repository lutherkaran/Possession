using System.Collections.Generic;
using UnityEngine;

public class AIPath<T> : MonoBehaviour where T : Entity
{
    [Header("Pathfinding Properties")]
    [SerializeField] private List<Transform> Waypoints = new List<Transform>();

    public Vector3 GetRandomPathPosition()
    {
        return Waypoints[Random.Range(0, Waypoints.Count - 1)].position;
    }
}
