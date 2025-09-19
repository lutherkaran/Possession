using System.Collections.Generic;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    public List<Transform> Waypoints = new List<Transform>();

    [SerializeField] private Enemy enemy;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.CanMove()) return;
        
        else
        {
            
        }
    }
}
