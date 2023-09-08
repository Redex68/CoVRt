using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class GuardNavigation : MonoBehaviour
{
    /// <summary> How long the  guard will wait before going to the next waypoint </summary>
    [SerializeField]
    float delay = 1f;
    /// <summary> A list of waypoints for the guard's patroll path. Will randomly choose a point to walk towards. </summary>
    [SerializeField]
    List<Transform> waypoints = new List<Transform>();
    
    private NavMeshAgent agent;
    private bool waiting = false;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if(waypoints.Count == 0) Debug.LogError("Guard has no waypoints");
    }

    // Update is called once per frame
    void Update()
    {
        if (!waiting && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {            
                if(waypoints.Count != 0)
                    StartCoroutine(pickNewWaypoint());
            }
        }
    }

    private IEnumerator pickNewWaypoint() {
        waiting = true;
        yield return new WaitForSeconds(delay);
        
        int indx = UnityEngine.Random.Range(0, waypoints.Count);
        agent.SetDestination(waypoints[indx].position);
        waiting = false;
    }
}