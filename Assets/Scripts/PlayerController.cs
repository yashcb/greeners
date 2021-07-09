using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PhysicsObject
{
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;

  //private SpriteRenderer spriteRenderer;
  private Animator animator;
    void Awake(){
      //  spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis ("Horizontal");

            if (Input.GetButtonDown("Jump") && grounded)
            {
                velocity.y = jumpTakeOffSpeed;
            }

        /* bool isFlipped = (spriteRenderer.flipX ? (move.x > 0.01 ) : (move.x < 0.01) );
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
