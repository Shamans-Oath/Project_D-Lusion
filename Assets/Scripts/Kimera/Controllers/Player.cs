using Features;
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

    [Header("Components")]
    public Transform dashPoint;

    private void OnEnable()
    {
        SearchFeature<Life>().OnDeath += OnDeath;
    }

    private void OnDisable()
    {
        SearchFeature<Life>().OnDeath -= OnDeath;
    }

    public override void Setup()
    {
        playerCamera = Camera.main;

        base.Setup();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        inputDirection = new Vector2(direction.y, direction.x);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed) CallFeature<Features.Jump>();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed) CallFeature<Features.Dash>(new Setting("dashPoint", dashPoint.position, Setting.ValueType.Vector3));
    }

    public void OnDeath()
    {
        CallFeature<Ragdoll>(new Setting("ragdollActivation", true, Setting.ValueType.Bool));
        ToggleActive(false);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed) CallFeature<CombatAnimator>(new Setting("combatCondition", "normal", Setting.ValueType.String));
    }

    public void OnSpecialAttack(InputAction.CallbackContext context)
    {
        if (context.performed) CallFeature<CombatAnimator>(new Setting("combatCondition", "special", Setting.ValueType.String));
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        if (context.performed) CallFeature<Block>(new Setting("toggleBlock", true, Setting.ValueType.Bool));
        if (context.canceled) CallFeature<Block>(new Setting("toggleBlock", false, Setting.ValueType.Bool));
    }
}
