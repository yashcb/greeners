using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerController : PhysicsObject
{
    private static PlayerController instance;
    public bool isAttacking;
    private Animator animator;

    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;
    public int playerHealth;
    public int playerMaxHealth;
    private float playerAttackRange;
    public float maxAttackRange;
    public bool canMove;

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
        canMove = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, maxAttackRange);
    }

    protected override void ComputeVelocity()
    {
        if (canMove)
        {
            Vector2 move = Vector2.zero;

            move.x = Input.GetAxis("Horizontal");
            if (Input.GetButtonDown("Jump") && grounded)
            {
                velocity.y = jumpTakeOffSpeed;
            }

            Vector3 characterScale = transform.localScale;

            if (move.x < 0)
            { characterScale.x = -1; }
            if (move.x > 0)
            { characterScale.x = 1; }
            transform.localScale = characterScale;

            if (Input.GetButtonDown("Fire1"))
                isAttacking = true;

            else if (Input.GetButtonUp("Fire1"))
                isAttacking = false;

            animator.SetBool("isAttacking", isAttacking);
            animator.SetBool("grounded", grounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
            animator.SetFloat("velocityY", Mathf.Abs(velocity.y));

            targetVelocity = move * maxSpeed;
        }
        else
        {
            targetVelocity = Vector2.zero;
        }
            playerAttackRange = AIController.EnemyInstance.gameObject.transform.position.x - transform.position.x;
    }

   
    public void Hit()
    {
        playerHealth -= 1;
        if (playerHealth <= 0)
            Die();

        Debug.Log(gameObject.name + " was hit!"); 
    }

   public void EnemyHurt()
   {
        if (Mathf.Abs(playerAttackRange) <= maxAttackRange && isAttacking)
        {
            AIController.EnemyInstance.Hit();
        }
    }

    public void Die()
    {
        Debug.Log("Player Died!");
        animator.SetTrigger("hurt");
        StartCoroutine(DisablePlayer());
    }

    IEnumerator DisablePlayer()
    {
        gameObject.GetComponent<Rigidbody2D>().simulated = false;
        gameObject.GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
        AIController.EnemyInstance.enemy = AIController.EnemyType.Bug;
    }
}
