using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity, IPossessible
{
    Rigidbody rb;
    IPossessible possessed;
    private Vector3 moveInput;
    private float speed = 10;

    void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    public void Update()
    {
        if (PossessionManager.currentlyPossessed == this.possessed)
        {
            PlayerControlling();
        }

    }

    private void PlayerControlling()
    {
        Jump();
        Movement();
        Attack();
    }

    public override void Attack()
    {

    }

    public override bool IsAlive()
    {
        return true;
    }

    public override void Jump()
    {

    }

    public override void Movement()
    {
        moveInput = Vector3.zero;
        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), .0f, Input.GetAxisRaw("Vertical")).normalized;
        transform.Translate(moveInput * speed * Time.deltaTime);
    }

    public void Possess()
    {
        Debug.Log("Possessing..." + this.gameObject);
        possessed = PossessionManager.ToPossess(this);
    }

    public void UnPossess()
    {
        Debug.Log("Un-Possessing..." + this.gameObject);
        //PossessionManager.UnPossessing();
        //FindAnyObjectByType<Player>().Possessed();
    }
}
