using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    List<Entity> entities = new List<Entity>();

    // Start is called before the first frame update
    private void Awake()
    {
       entities.AddRange(GameObject.FindObjectsOfType<Entity>());
       
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
