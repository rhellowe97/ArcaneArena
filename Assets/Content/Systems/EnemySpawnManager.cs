using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField]
    private List<Transform> spawnLocations;

    [SerializeField] private GameObject enemy;

    [SerializeField] private float spawnRate = 15f;

    private float nextSpawnTime = 0f;

    private float spawnTimer = 0f;

    private void Update()
    {
        spawnTimer += Time.deltaTime;

        if ( spawnTimer >= nextSpawnTime )
        {
            spawnTimer = 0f;

            GetSpawnTime();

            Transform chosenSpawn = spawnLocations[Random.Range( 0, spawnLocations.Count )];

            Instantiate( enemy, chosenSpawn.position, chosenSpawn.rotation );
        }
    }

    private void GetSpawnTime()
    {
        nextSpawnTime = Random.Range( spawnRate - 3, spawnRate + 3 );
    }
}
