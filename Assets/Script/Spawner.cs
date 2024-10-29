using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject[] enemyPrefabs;     // Tableau contenant les différents préfabriqués d'ennemis
    public int enemiesPerRound = 5;        // Nombre d'ennemis à faire apparaître par round
    public int totalRounds = 3;            // Nombre total de rounds
    public float roundTimer = 5f;          // Durée du timer entre les rounds
    public float spawnDelay = 1f;          // Délai entre chaque ennemi dans un round

    [Header("Spawn Area Settings")]
    public Vector3 spawnAreaSize = new Vector3(10, 0, 10);  // Taille de la zone de spawn

    void Start()
    {
        StartCoroutine(SpawnRounds());
    }

    private IEnumerator SpawnRounds()
    {
        for (int round = 0; round < totalRounds; round++)
        {
            yield return StartCoroutine(SpawnEnemies());
            yield return new WaitForSeconds(roundTimer); // Attendre la durée du round
        }
    }

    private IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < enemiesPerRound; i++)
        {
            Vector3 spawnPosition = GetRandomPosition();
            GameObject enemyPrefab = GetRandomEnemyPrefab();
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(spawnDelay); // Attendre le délai avant de spawn le prochain ennemi
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