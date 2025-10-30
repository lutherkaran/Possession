using System.Collections.Generic;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    [Header("Pathfinding Properties")]
    [SerializeField] private List<Transform> Waypoints = new List<Transform>();

    public Vector3 GetRandomPathPosition()
    {
        return Waypoints[Random.Range(0, Waypoints.Count - 1)].position;
    }
}
