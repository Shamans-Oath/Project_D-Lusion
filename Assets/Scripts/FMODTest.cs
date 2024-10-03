using Features;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FMODTest : MonoBehaviour
{
    public float minRPM = 0;
    public float maxRPM = 2000;

    float currentSpeed;
    float maxSpeedValue;

    FMODUnity.StudioEventEmitter emitter;
    public Movement movement;
    public StatsHandler statsHandler;
    public Furry furry;

    // Start is called before the first frame update
    void Start()
    {
        emitter = GetComponent<FMODUnity.StudioEventEmitter>();
        maxSpeedValue = statsHandler.startMaxSpeed + furry.furryMax * statsHandler.maxSpeedIncrementPerFurry;
    }

    // Update is called once per frame
    void Update()
    {
        currentSpeed = movement.maxSpeed;
        Debug.Log(currentSpeed);

        float ratio = Mathf.InverseLerp(statsHandler.startMaxSpeed, maxSpeedValue, currentSpeed);
        float effectiveRPM = Mathf.Lerp(minRPM,maxRPM, ratio);
        emitter.SetParameter("RPM", effectiveRPM);
    }
}
