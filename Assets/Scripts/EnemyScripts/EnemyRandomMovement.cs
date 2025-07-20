using UnityEngine;
using UnityEngine.AI;

public class EnemyRandomMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    public float range;
    public Transform centrePoint;

    void Start()
    {
        agent = GetComponentInChildren<NavMeshAgent>();
    }

    void Update()
    {
        
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 point;
            if (RandomPoint(centrePoint.position, range, out point))
            {
                Debug.DrawRay(point, Vector3.up, Color.yellow, 1f);
                agent.SetDestination(point);
            }
        }
    }

    bool RandomPoint(Vector3 centre, float range, out Vector3 result)
    {
        Vector3 randomPoint = centre + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, range, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }
        result = Vector3.zero;
        return false;
    }
    void OnDrawGizmos()
    {
        if (centrePoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(centrePoint.position, range);
        }
    }
}
