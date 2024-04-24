using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PatternBehavior()
    {
        switch(states)
        {
            case States.Orbitating:

            break;

            case States.StandBy:

            break;

            case States.Following:

            break;
            
            case States.Attacking:
                
            break;
        }
    }

    IEnumerator hitstunCooldown(float cooldown)
    {
        inHitstun = true;

        yield return new WaitForSeconds(cooldown);

        inHitstun = false;
    }
}
