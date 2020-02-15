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

    Team team;
    private float nextSpawnTime;

    void Start()
    {
        team = GetComponent<Team>();
        nextSpawnTime = firstSpawnTime;
    }

    void Update()
    {
        if (nextSpawnTime < Time.time)
        {
            foreach (var spawner in spawners)
            {
                var minion = Instantiate(spawner.enemyType, spawner.spawnMarker);
                minion.GetComponent<Team>().faction = team.faction;
                minion.GetComponent<EnemyController>().SetPath(waypointPath);
            }
            nextSpawnTime += spawnDelay;
        }
    }
}
