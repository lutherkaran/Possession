using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{ 
    public abstract void Movement();
    public abstract void Jump();
    public abstract void Attack();
    public abstract bool IsAlive();

}
