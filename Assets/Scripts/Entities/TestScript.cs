using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField]
    LifeClass health;

    public int amount;
    public float percentage;


    public Transform offset;
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        health.currentHealth = health.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        HealthTest();
        AngleTest();
    }

    void AngleTest()
    {
        Vector3 offsetToPlayer = offset.position - target.transform.position;
        Vector3 camToPlayer = Camera.main.transform.position - target.transform.position;

        if(Input.GetKeyDown(KeyCode.K))
        {
            float angle = Vector3.Angle(offsetToPlayer, camToPlayer);

            Debug.Log(angle);
        }
    }

    void HealthTest()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            health.AddMaxHealth(amount, true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            health.ReduceMaxHealth(amount, true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            health.GainHealth(amount);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            health.LooseHealth(amount);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            health.PercentageHealthGain(percentage);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            health.PercentageHealthLoss(percentage);
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            health.AddPercetangeMaxHealth(percentage);
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            health.ReducePercetangeMaxHealth(percentage);
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            health.AddPercetangeMaxHealthNR(percentage);
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            health.ReducePercetangeMaxHealthNR(percentage);
        }
    }
}
