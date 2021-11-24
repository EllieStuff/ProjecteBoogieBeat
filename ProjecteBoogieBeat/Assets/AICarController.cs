using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICarController : MonoBehaviour
{
    const float MARGIN = 5.5f;

    public int currLap = 0;

    [SerializeField] Transform destinationSet;
    [SerializeField] private float startDelay = 6.0f;
    [SerializeField] private float stopSpeed = 1.0f;

    private Vector3[] destinations;
    private NavMeshAgent navMeshAgent;
    private int currDestination = 0;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        destinations = new Vector3[destinationSet.childCount];
        for (int i = 0; i < destinations.Length; i++)
            destinations[i] = destinationSet.GetChild(i).position;

        StartCoroutine(StartDelayCoroutine());

    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, destinations[currDestination]) < MARGIN)
        {
            currDestination++;
            if (currDestination >= destinations.Length)
                currDestination = 0;

            navMeshAgent.destination = destinations[currDestination];
        }
    }


    public void ReachedGoal()
    {
        StartCoroutine(StopCarCoroutine());
    }


    IEnumerator StartDelayCoroutine()
    {
        yield return new WaitForSeconds(startDelay);
        navMeshAgent.destination = destinations[currDestination];
    }

    IEnumerator StopCarCoroutine()
    {
        while(navMeshAgent.speed > 0.0f)
        {
            yield return new WaitForEndOfFrame();
            navMeshAgent.speed -= stopSpeed * Time.deltaTime;
        }

        if (navMeshAgent.speed < 0.0f) navMeshAgent.speed = 0.0f;

    }

}
