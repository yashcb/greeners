using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : PhysicsObject
{
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;
    private Animator animator;
    private bool isAttacking;

    void Awake() {
        animator = GetComponent<Animator>();
    }

    private static PlayerController instance;
    public static PlayerController Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<PlayerController>();
            return instance;
        }
    }
    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis ("Horizontal");

        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = jumpTakeOffSpeed;
        }

        Vector3 characterScale = transform.localScale;
        if (move.x < 0)
        {
            characterScale.x = -1;
        } 
        if (move.x > 0)
        {
            characterScale.x = 1;
        }
        transform.localScale = characterScale;

        
        if (Input.GetButtonDown("Fire1"))
        {
            isAttacking = true;
        }
        else if(Input.GetButtonUp("Fire1"))
            isAttacking = false;

        animator.SetBool("grounded", grounded);
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x )/ maxSpeed);
        animator.SetFloat("velocityY", Mathf.Abs(velocity.y ));
        animator.SetBool("isAttacking", isAttacking);
        targetVelocity = move * maxSpeed;
    }
}
