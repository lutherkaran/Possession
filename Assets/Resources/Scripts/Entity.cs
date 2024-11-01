using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    protected PlayerController player;

    public Vector3 moveDirection = Vector3.zero;
    public Vector3 velocity = Vector3.zero;

    public float jumpHeight = 1.5f;
    public float gravity = -9.8f;
    public float speed;

    public bool sprinting = false;
    public bool isGrounded = true;

    public void SetPlayer(PlayerController Player)
    {
        player = Player;
    }

    public virtual void ProcessMove(Vector2 input)
    {
        moveDirection.x = input.x;
        moveDirection.z = input.y;
    }

    public virtual void ProcessJump()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -3f * gravity);
        }
    }

    public virtual void Sprint()
    {
        sprinting = !sprinting;
        speed = sprinting ? 10f : 5f;
    }

    public abstract void Attack();
    public abstract bool IsAlive();
}
//TODO Any enitity should be able to use these methods
//TODO Depends which entity is currently possessed, get it's GameObject's type and then make it perform movement's actions.