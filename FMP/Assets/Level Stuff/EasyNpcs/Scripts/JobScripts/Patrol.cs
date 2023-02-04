using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : Work
{
    public Transform point1;
    public Transform point2;
    public Transform point3;

    NavMeshAgent agent;
    Vector3 currentPosition;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        StartCoroutine(PatrolPoints());
    }

    IEnumerator PatrolPoints()
    {
        agent.SetDestination(point1.position);
        currentPosition = point1.position;
        yield return new WaitUntil(ReachedDestination);
        yield return new WaitForSeconds(2);

        agent.SetDestination(point2.position);
        currentPosition = point2.position;
        yield return new WaitUntil(ReachedDestination);
        yield return new WaitForSeconds(2);

        agent.SetDestination(point3.position);
        currentPosition = point3.position;
        yield return new WaitUntil(ReachedDestination);
        yield return new WaitForSeconds(2);

        StartCoroutine(PatrolPoints());
    }

    bool ReachedDestination()
    {
        if (Vector3.Distance(currentPosition, transform.position) < 0.15f)
            return true;
        else
            return false;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
