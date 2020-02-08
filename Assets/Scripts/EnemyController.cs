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
            position.y = Mathf.Lerp(position.y, -10.0f, Time.deltaTime);
            transform.position = position;
            if (position.y <= -10.0f)
            {
                Destroy(gameObject);
            }
        }
        else if (health.IsAlive())
        {
            Vector3 waypoint = path.GetWaypoint(nextPathIndex);
            if (Vector2.Distance(waypoint.XZ(), transform.position.XZ()) <= arrivalDistance)
            {
                nextPathIndex = path.GetNextIndex(nextPathIndex);
            }
            agent.SetDestination(path.GetWaypoint(nextPathIndex));
        }
        else
        {
            GetComponent<Collider>().enabled = false;
            agent.enabled = false;
            isDying = true;
        }
    }

    public void SetPath(WaypointPath waypointPath)
    {
        path = waypointPath;
        nextPathIndex = 0;
    }
}
