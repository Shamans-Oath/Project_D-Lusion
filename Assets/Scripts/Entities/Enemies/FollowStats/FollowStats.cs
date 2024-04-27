using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FollowStats", menuName = "Enemies/FollowStats", order = 1)]
public class FollowStats : ScriptableObject
{
    public float Speed;
    public float angularSpeed;
    public float Acceleration;
    public float stoppingDistance;
}
