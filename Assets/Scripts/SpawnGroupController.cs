using System.Collections.Generic;
using UnityEngine;

public class SpawnGroupController : MonoBehaviour
{
    [System.Serializable]
    public struct Spawner
    {
        public GameObject enemyType;
        public Transform spawnMarker;
    }

    public float firstSpawnTime;
    public float spawnDelay;
    public WaypointPath waypointPath;
    public List<Spawner> spawners;

    private float nextSpawnTime;

    void Start()
    {
        nextSpawnTime = firstSpawnTime;
    }

    void Update()
    {
        if (nextSpawnTime < Time.time)
        {
            foreach (var spawner in spawners)
            {
                var enemy = Instantiate(spawner.enemyType, spawner.spawnMarker);
                enemy.GetComponent<EnemyController>().SetPath(waypointPath);
            }
            nextSpawnTime += spawnDelay;
        }
    }
}
