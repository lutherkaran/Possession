using System.Collections.Generic;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    [Header("Pathfinding Properties")]
    [SerializeField] private List<Transform> Waypoints = new List<Transform>();

    public List<Transform> GetPath()
    {
        return Waypoints;
    }

    public Vector3 GetRandomPathPosition()
    {
        return Waypoints[Random.Range(0, Waypoints.Count - 1)].position;
    }

    public List<Transform> GetRandomPath()
    {
        List<Transform> path = Waypoints;

        int numberOfWaypoints = path.Count;

        while (numberOfWaypoints > 0)
        {
            numberOfWaypoints--;

            int randomNumber = Random.Range(0, numberOfWaypoints);

            path[numberOfWaypoints] = Waypoints[randomNumber];
            Waypoints[randomNumber] = Waypoints[numberOfWaypoints];
            Waypoints[numberOfWaypoints] = path[numberOfWaypoints];
        }

        return path;
    }
}
