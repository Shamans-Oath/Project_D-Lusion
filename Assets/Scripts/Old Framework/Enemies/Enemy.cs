using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum States
    {
        Orbitating,
        StandBy,
        Following,
        Attacking
    }

    public States states;

    public LayerMask Target;

    public bool inHitstun;

    public NavMeshAgent cmp_Agent;

    public FollowStats[] agentValues;

    public Transform followTarget;
    public Transform lookingTarget;
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(inHitstun == false)
        {
            EnemyMovement();
        }        
    }

    public void BehaviorChange(States newState, Transform target, Transform lookingAt)
    {
        states = newState;

        followTarget = target;
        lookingTarget = lookingAt;

       switch (states)
        {
            case States.Orbitating:
                cmp_Agent.speed = agentValues[0].Speed;
                cmp_Agent.angularSpeed = agentValues[0].angularSpeed;
                cmp_Agent.acceleration = agentValues[0].Acceleration;
                cmp_Agent.stoppingDistance = agentValues[0].stoppingDistance;
            break;

            case States.StandBy:
                cmp_Agent.speed = agentValues[1].Speed;
                cmp_Agent.angularSpeed = agentValues[1].angularSpeed;
                cmp_Agent.acceleration = agentValues[1].Acceleration;
                cmp_Agent.stoppingDistance = agentValues[1].stoppingDistance;
            break;

            default:
                Debug.Log("Estas usando la funcion de moverse mirando a un objetivo, los otros dos estados no requieren de un lookingAt");
            break;
        }
    }

    public void BehaviorChange(States newState, Transform target)
    {
        states = newState;

        followTarget = target;
        lookingTarget = null;

        switch(states)
        {
            case States.Following:
                cmp_Agent.speed = agentValues[2].Speed;
                cmp_Agent.angularSpeed = agentValues[2].angularSpeed;
                cmp_Agent.acceleration = agentValues[2].Acceleration;
                cmp_Agent.stoppingDistance = agentValues[2].stoppingDistance;
            break;
            
            case States.Attacking:
                cmp_Agent.speed = agentValues[3].Speed;
                cmp_Agent.angularSpeed = agentValues[3].angularSpeed;
                cmp_Agent.acceleration = agentValues[3].Acceleration;
                cmp_Agent.stoppingDistance = agentValues[3].stoppingDistance;
            break;

            default:
                Debug.Log("Estas usando la funcion de moverse hacia un punto, si quieres que el objeto mire hacia otro lado debes agregar un lookingAt");
            break;
        }
    }    

    public void EnemyMovement()
    {   
        if(followTarget != null)
        {
            cmp_Agent.SetDestination(followTarget.position);

            if(lookingTarget != null)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookingTarget.transform.position - transform.position);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }        
    }

    IEnumerator hitstunCooldown(float cooldown)
    {
        inHitstun = true;

        yield return new WaitForSeconds(cooldown);

        inHitstun = false;
    }
}
