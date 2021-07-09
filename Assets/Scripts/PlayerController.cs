using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PhysicsObject
{
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;
    //private GameObject player;

    //private SpriteRenderer spriteRenderer;
    private Animator animator;
    void Awake(){
       // spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
        //player = GetComponent<GameObject>();
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
        if (Input.GetAxis("Horizontal") < 0)
        {
            characterScale.x = -1;
        } 
        if (Input.GetAxis("Horizontal") > 0)
        {
            characterScale.x = 1;
        }
        transform.localScale = characterScale;

        /*bool isFlipped = (spriteRenderer.flipX ? (move.x > 0.01 ) : (move.x < 0.01) );
        if (isFlipped)
        {
            isFlipped = !isFlipped;
        }*/

        animator.SetBool("grounded", grounded);
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x )/ maxSpeed);
        animator.SetFloat("velocityY", Mathf.Abs(velocity.y ));
        targetVelocity = move * maxSpeed;
    }
}
