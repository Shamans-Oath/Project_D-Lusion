using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField]
    LifeClass health;

    public int amount;
    public float percentage;

    // Start is called before the first frame update
    void Start()
    {
        health.currentHealth = health.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            health.AddMaxHealth(amount);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            health.ReduceMaxHealth(amount);
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
    }
}
