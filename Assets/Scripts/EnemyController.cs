using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Tooltip("This should be bigger than Nav Mesh Agent's stopping distance")]
    public float arrivalDistance;

    private NavMeshAgent agent;
    private WaypointPath path;
    private int nextPathIndex = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Vector3 waypoint = path.GetWaypoint(nextPathIndex);
        if (Vector2.Distance(waypoint.XZ(), transform.position.XZ()) <= arrivalDistance)
        {
            nextPathIndex = path.GetNextIndex(nextPathIndex);
        }
        agent.SetDestination(path.GetWaypoint(nextPathIndex));
    }

    public void SetPath(WaypointPath waypointPath)
    {
        path = waypointPath;
        nextPathIndex = 0;
    }
}
