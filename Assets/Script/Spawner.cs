using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject[] enemyPrefabs;     // Tableau contenant les différents préfabriqués d'ennemis
    public int numberOfEnemies = 5;       // Nombre d'ennemis à faire apparaître

    [Header("Spawn Area Settings")]
    public Vector3 spawnAreaSize = new Vector3(10, 0, 10);  // Taille de la zone de spawn

    void Start()
    {
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector3 spawnPosition = GetRandomPosition();
            GameObject enemyPrefab = GetRandomEnemyPrefab();
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private Vector3 GetRandomPosition()
    {
        float x = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
        float z = Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2);
        Vector3 spawnPosition = transform.position + new Vector3(x, 0, z);
        return spawnPosition;
    }

    private GameObject GetRandomEnemyPrefab()
    {
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        return enemyPrefabs[randomIndex];
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, spawnAreaSize);
    }
}
