using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> treasurePrefabs;
    public List<GameObject> spawnPoints;

    Vector3 pos;
    [SerializeField]
    float setSpawnTimer = 10.0f, spawnTimer = 0, stopSpawning = 120;
    public float spawnLimit = 20;
    bool canSpawn = false;
    Vector3 randSpawn = new Vector3(0, 0);
    float xPoint , yPoint;
    // gets the spawn timer from the editor
    private void Start()
    {
        spawnTimer = setSpawnTimer;
    }

    private void Update()
    {
        //will begin immediatly and will spawn the corresponding treasure
        if (stopSpawning > 0)
        {
            stopSpawning -= Time.deltaTime;
            spawnTimer -= Time.deltaTime;
        }
        if (spawnLimit > 0)
        {
            if (spawnTimer < 0 && stopSpawning > 0)
            {
                SpawnTreasure();
                spawnTimer = setSpawnTimer;
                spawnLimit -= 1;
            }
        }

    }


    void SpawnTreasure()
    {
        //this decides where and what treasure is spawned in
        int randomSpawner = Random.Range(0, spawnPoints.Count);
        pos = spawnPoints[randomSpawner].transform.position;
        if (spawnPoints[randomSpawner].GetComponent<SpawnerRange>() != null)
        {
            xPoint = spawnPoints[randomSpawner].GetComponent<SpawnerRange>().x/2;
            yPoint = spawnPoints[randomSpawner].GetComponent<SpawnerRange>().y/2;
            treasurePrefabs = spawnPoints[randomSpawner].GetComponent<SpawnerRange>().treasurePrefabs;
        }

        int failSpawn = 0;
        int randTreasure = Random.Range(0, treasurePrefabs.Count);
        // while the item can have a spot spawn, it can spawn based on the parameters given of the area it can
        while (!canSpawn)
        {
            randSpawn = new Vector3(Random.Range(pos.x - xPoint, pos.x + xPoint),
                pos.y, Random.Range(pos.z - yPoint, pos.z + yPoint));
            if (randSpawn.x < -0 || randSpawn.x > 0 ||
                randSpawn.z < -0 || randSpawn.z > 0)
            {
                Collider[] hitColliders = Physics.OverlapSphere(randSpawn, 1);
                foreach (var colliders in hitColliders)
                {
                    if (colliders.gameObject.layer == 6)
                    {
                        canSpawn = true;

                    }
                }
            }
            failSpawn += 1;
            if (failSpawn > 5)
            {
                canSpawn = false;
                return;
            }
        }
        //spawns the treasure in
        GameObject treasure = Instantiate(treasurePrefabs[randTreasure], randSpawn, spawnPoints[randomSpawner].transform.rotation, spawnPoints[randomSpawner].transform);
        treasure.GetComponent<SpawnableObjects>().Spawner = gameObject;
        canSpawn = false;
    }

}
