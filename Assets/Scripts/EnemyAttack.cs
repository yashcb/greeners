using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6 && AIController.EnemyInstance.hasDied == false)
        {
            AIController.EnemyInstance.isAttacking= true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            AIController.EnemyInstance.isAttacking = false;
        }
    }
}
