using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : PhysicsObject
{
   
    public float aiMaxSpeed = 7;
    public float aiDirection = 1;
    public float aiChangeDirectionEase = 1;
    public float followRange;
    public GameObject attackHitBox;
 
    [SerializeField] enum EnemyType { Bug, Zombie };
    [SerializeField] EnemyType enemy;
    [SerializeField] private Animator aiAnimator;
    [SerializeField] private Vector2 rayCastOffset;
    private float aiDirectionSmooth = 1;
    private float playerDifference;
    private bool followPlayer;
    private RaycastHit2D leftLedge;
    private RaycastHit2D rightLedge;
    private int enemyHealth = 1;
    public static bool isAttacking = false;
    public float attackInterval;

    public void Start()
    {
        aiAnimator = GetComponent<Animator>();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, followRange);
    }

    protected override void ComputeVelocity()
    {
        if (attackInterval > 0)
        {
            attackInterval -= Time.deltaTime;
        }

        Vector2 aiMove = Vector2.zero;
        playerDifference = PlayerController.Instance.gameObject.transform.position.x - transform.position.x;
        aiDirectionSmooth += (aiDirection - aiDirectionSmooth) * Time.deltaTime * aiChangeDirectionEase;
        aiMove.x = 1 * aiDirectionSmooth;

        // Flip sprite
        if (aiMove.x > 0.01)
        {
            transform.localScale =
                    new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }

        else if (aiMove.x < 0.01)
        {
            transform.localScale =
                    new Vector3(1, transform.localScale.y, transform.localScale.z);
        }

        // Enemy type
        if (enemy == EnemyType.Zombie)
        {
            if ((Mathf.Abs(playerDifference) < followRange))
            {
                followPlayer = true;
            }
            else
                followPlayer = false;
        }

        // Direction
        if (followPlayer)
        {
            if (playerDifference < 0)
                aiDirection = -1;
            else
                aiDirection = 1;
        }
        else
        {   aiDirectionSmooth = aiDirection; }


        // Check for ledges
        if (!followPlayer)
        {
            // Right ledge
            rightLedge = Physics2D.Raycast(new Vector2(transform.position.x + rayCastOffset.x, transform.position.y), Vector2.down, .5f);
            Debug.DrawRay(new Vector2(transform.position.x + rayCastOffset.x, transform.position.y), Vector2.down, Color.blue);
            if (rightLedge.collider == null)
            { aiDirection = -1; }

            // Left ledge
            leftLedge = Physics2D.Raycast(new Vector2(transform.position.x - rayCastOffset.x, transform.position.y), Vector2.down, .5f);
            Debug.DrawRay(new Vector2(transform.position.x - rayCastOffset.x, transform.position.y), Vector2.down, Color.blue);
            if (leftLedge.collider == null)
            { aiDirection = 1; }
        }
        

        if (isAttacking && attackInterval <= 0)
        {
            aiAnimator.SetBool("isAttacking", true);
        }
        else
            aiAnimator.SetBool("isAttacking", false);

        // Movement
        if (isAttacking)
        {   targetVelocity = Vector2.zero;  }
        else
        {
            aiAnimator.SetFloat("velocityx", Mathf.Abs(velocity.x) / aiMaxSpeed);
            targetVelocity = aiMove * aiMaxSpeed;
        }
       
    }

    public void Hit()
    {
        Debug.Log(gameObject.name + " was hit");
        if (enemyHealth <= 0)
        {
            aiAnimator.SetTrigger("hurt");
            Die();
        }
        else
            enemyHealth -= 1;
    }

    public void  PlayerHurt()
    {
        PlayerController.Instance.Hit();
    }

    public void Die()
    {
        Debug.Log("Enemy died!");
        gameObject.GetComponent<Collider2D>().enabled = false;
       // Destroy(gameObject);
    }

    

}
