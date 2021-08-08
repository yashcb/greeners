using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerController : PhysicsObject
{
    private static PlayerController instance;
    private bool isAttacking;
    private Animator animator;

    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;
    public int playerHealth;
    public int playerMaxHealth;

    [SerializeField] GameObject attackHit;
    public static PlayerController Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<PlayerController>();
            return instance;
        }
    }

    private void Start()
    {
        playerHealth = playerMaxHealth;
        animator = GetComponent<Animator>();
    }

    protected override void ComputeVelocity()
    {
       
            Vector2 move = Vector2.zero;

            move.x = Input.GetAxis("Horizontal");

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
            else if (Input.GetButtonUp("Fire1"))
                isAttacking = false;

            animator.SetBool("grounded", grounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
            animator.SetFloat("velocityY", Mathf.Abs(velocity.y));
            animator.SetBool("isAttacking", isAttacking);
            targetVelocity = move * maxSpeed;
        
    }

    public void Hit()
    {
        if (playerHealth <= 0)
        {
            Die();
        }
        
        Debug.Log(gameObject.name + " was hit!");
        playerHealth -= 1;
    }

    public void Die()
    {
        Debug.Log("Player Died!");
        animator.SetTrigger("hurt");
        
        gameObject.SetActive(false);
        SceneManager.LoadScene("Gameplay");
        //gameObject.GetComponent<Collider2D>().enabled = false;
        //Destroy(gameObject);
    }
}
