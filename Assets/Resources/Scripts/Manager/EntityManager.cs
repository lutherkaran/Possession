using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    List<Entity> entityList = new List<Entity>();

    // Start is called before the first frame update
    private void Awake()
    {
        entityList.AddRange(GameObject.FindObjectsOfType<Entity>());
        SetEntityParent();
    }

    private void SetEntityParent()
    {
        foreach (Entity entity in entityList)
        {
            entity.transform.SetParent(this.transform);
        }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    { 

    }
}
