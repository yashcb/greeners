using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : PhysicsObject
{
    [SerializeField] enum EnemyType { Bug, Zombie };
    [SerializeField] EnemyType enemy;
    public float aiMaxSpeed = 7;
    public float aiDirection = 1;
    public float aiChangeDirectionEase = 1;
    private float aiDirectionSmooth = 1;
    private float playerDifference;
    [SerializeField] private Animator aiAnimator;

    private bool followPlayer;
    public float followRange;
    [SerializeField] private Vector2 rayCastOffset;
    //[SerializeField] private LayerMask layerMask;
    private RaycastHit2D leftLedge;
    private RaycastHit2D rightLedge;

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
        // Direction
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
        {
            aiDirectionSmooth = aiDirection;
        }


        // Check for ledges
        if (!followPlayer)
        {
            // Left ledge
            leftLedge =
                Physics2D.Raycast(new Vector2(transform.position.x - rayCastOffset.x, transform.position.y), Vector2.down, .5f);
            Debug.DrawRay(new Vector2(transform.position.x - rayCastOffset.x, transform.position.y), Vector2.down, Color.blue);

            if (leftLedge.collider == null)
            { aiDirection = 1; }

            // Right ledge
            rightLedge =
                Physics2D.Raycast(new Vector2(transform.position.x + rayCastOffset.x, transform.position.y), Vector2.down, .5f);
            Debug.DrawRay(new Vector2(transform.position.x + rayCastOffset.x, transform.position.y), Vector2.down, Color.blue);

            if (rightLedge.collider == null)
            { aiDirection = -1; }

        }

        // Movement
        aiAnimator.SetFloat("velocityx", Mathf.Abs(velocity.x) / aiMaxSpeed);
        targetVelocity = aiMove * aiMaxSpeed;
    }
}
