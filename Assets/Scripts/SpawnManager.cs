using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject enemyPrefab;

    private void Start()
    {
        Debug.Log("Waves started!");
        SpawnEnemies();
    }

    private void OnEnable()
    {
        AIController.OnEnemyKilled += SpawnEnemies;
    }

    private void SpawnEnemies()
    {
        int randRate = Mathf.RoundToInt(Random.Range(0f, spawnPoints.Length - 1));

        Instantiate(enemyPrefab, spawnPoints[randRate].transform.position, Quaternion.identity);
    }
}
