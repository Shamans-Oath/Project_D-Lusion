using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Controller, InputEntity, KineticEntity, TerrainEntity, SpecialTerrainEntity, CombatEntity, FurryEntity, LivingEntity, StunEntity
{
    //States
    //Input
    public Vector2 inputDirection { get; set; }
    public Vector3 playerForward { get; set; }
    public Camera playerCamera { get; set; }
    //Kinetic
    public float currentSpeed { get; set; }
    //Terrain
    public bool onGround { get; set; }
    public bool onSlope { get; set; }
    //SpecialTerrain
    public bool onLadder { get; set; }
    //Combat
    public bool block { get; set; }
    public bool parry { get; set; }
    public int attack { get; set; }
    //Furry
    public float furryCount { get; set; }
    //Living
    public int currentHealth { get; set; }
    public int maxHealth { get; set; }
    // Stun
    public bool isStunned { get; set; }

    public override void Setup()
    {
        playerCamera = Camera.main;

        base.Setup();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputDirection = context.ReadValue<Vector2>();
    }
}
