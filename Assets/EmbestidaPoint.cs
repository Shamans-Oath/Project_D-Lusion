using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EmbestidaPoint : MonoBehaviour
{
    private NavMeshAgent agent;   
    public float moveSpeed = 5f;       
    public float speedVariation = 2f;   
    public float directionChangeInterval = 1f; 
    public float wanderRadius = 10f;     

    private float timeSinceLastDirectionChange = 0f;



    void Start()
    {
        StartCoroutine(Borrar());
        if (agent == null)
        {
            agent = gameObject.GetComponent<NavMeshAgent>(); 

            if (agent == null)
            {
                Debug.LogError("No hay agente pe");

                enabled = false;
                return;
            }

        }
        agent.speed = moveSpeed + Random.Range(-speedVariation, speedVariation);
    }


    void Update()
    {

        timeSinceLastDirectionChange += Time.deltaTime;

        if (timeSinceLastDirectionChange >= directionChangeInterval)
        {
            SetRandomDestination();

            timeSinceLastDirectionChange = 0f;
        }

    }

    void SetRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;

        NavMeshHit navHit;
        if (NavMesh.SamplePosition(randomDirection, out navHit, wanderRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(navHit.position);
            agent.speed = moveSpeed + Random.Range(-speedVariation, speedVariation); 


        }


    }
    public  IEnumerator Borrar()
    {
        yield return new WaitForSeconds(14f);
        Destroy(gameObject);
    }
}
