using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Tooltip("This should be bigger than Nav Mesh Agent's stopping distance")]
    public float arrivalDistance;

    private NavMeshAgent agent;
    private Health health;
    private WaypointPath path;
    private int nextPathIndex = 0;
    private Vector3 waypointOffset;
    private bool isDying = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
    }

    void Update()
    {
        if (isDying)
        {
            Vector3 position = transform.position;
            position.y = Mathf.Lerp(position.y, position.y - 5.0f, Time.deltaTime);
            transform.position = position;
            if (position.y <= 0.0f)
            {
                Destroy(gameObject);
            }
        }
        else if (health.IsAlive())
        {
            Vector3 waypoint = path.GetWaypoint(nextPathIndex) + waypointOffset;
            if (Vector2.Distance(waypoint.XZ(), transform.position.XZ()) <= arrivalDistance)
            {
                nextPathIndex = path.GetNextIndex(nextPathIndex);
            }
            agent.SetDestination(path.GetWaypoint(nextPathIndex) + waypointOffset);
        }
        else
        {
            agent.enabled = false;
            isDying = true;
        }
    }

    public void SetPath(WaypointPath waypointPath)
    {
        path = waypointPath;
        nextPathIndex = 0;
        waypointOffset = transform.position - path.GetWaypoint(0);
        waypointOffset.y = 0.0f;
    }
}
