using Features;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Controller, InputEntity, KineticEntity, TerrainEntity, SpecialTerrainEntity, CombatEntity, FurryEntity, LivingEntity, StunEntity, FollowEntity
{
    //States
    //Input
    public PlayerInput input;
    public Vector2 inputDirection { get; set; }
    public Vector3 playerForward { get; set; }
    public Camera playerCamera { get; set; }
    //Kinetic
    public Vector3 speed { get; set; }
    public float currentSpeed { get; set; }
    public float maxSpeed { get; set; }
    //Terrain
    public bool onGround { get; set; }
    public bool onSlope { get; set; }
    //SpecialTerrain
    public bool onLadder { get; set; }
    //Combat
    public bool block { get; set; }
    public bool parry { get; set; }
    public int attack { get; set; }
    public int comboCount { get; set; }
    //Furry
    public float furryCount { get; set; }
    public float maxFurryCount { get; set; }
    public int furryCombo {  get; set; }
    //Living
    public int currentHealth { get; set; }
    public int maxHealth { get; set; }
    // Stun
    public bool isStunned { get; set; }

    // Dash Distance
    public float maxDashDistance { get; set; }

    //Follow Entity
    public GameObject target {  get; set; }

    [Header("Properties")]
    public bool triggerDownAttack;

    [Header("Components")]
    public Transform dashPoint;
    public Camera_System cameraSys;

    private void OnEnable()
    {
        GameManager.StateChanged += CheckInputState;
        SearchFeature<Life>().OnDeath += OnDeath;
    }

    private void OnDisable()
    {
        GameManager.StateChanged -= CheckInputState;
        SearchFeature<Life>().OnDeath -= OnDeath;
    }

    public override void Setup()
    {
        playerCamera = Camera.main;
        triggerDownAttack = false;
        base.Setup();
    }

    protected override void Update()
    {
        if (!active) return;

        if (onGround == true) triggerDownAttack = false;

        base.Update();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        inputDirection = new Vector2(direction.y, direction.x);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (block) return;
        if (context.performed)
        {
            CallFeature<Features.Jump>();
            StartCoroutine(SearchFeature<Life>().ImmunityCoroutine());
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (block) return;
        if (context.performed)
        {
            // SearchFeature<Rotation>().RotateTo(dashPoint.Position);
            //CallFeature<Features.Dash>(new Setting("dashPoint", dashPoint.Position, Setting.ValueType.Vector3));
            //CallFeature<Ragdoll>(new Setting("ragdollActivation", true, Setting.ValueType.Bool));
            SearchFeature<TimeFocusMode>().EnableFocus();
            cameraSys.lockSys.AimVisual(true);
        }
        if(context.canceled)
        {
            cameraSys.lockSys.AimVisual(false);
            SearchFeature<TimeFocusMode>().DisableFocus();
            if (maxDashDistance <= 0) maxDashDistance = 15;
            if (cameraSys.GetCameraLookat(maxDashDistance).HasValue)
            {
                dashPoint.position = cameraSys.GetCameraLookat(maxDashDistance).Value;
                CallFeature<Features.Dash>(new Setting("dashPoint", dashPoint.position, Setting.ValueType.Vector3));
            }
            
        }
    }

    public void OnDeath()
    {
        CallFeature<Ragdoll>(new Setting("ragdollActivation", true, Setting.ValueType.Bool));
        //ToggleActive(false);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (block) return;
        if (context.performed)
        {
            if (!onGround && !triggerDownAttack)
            {
                CallFeature<CombatAnimator>(new Setting("combatCondition", "attack-down", Setting.ValueType.String));
                CallFeature<AttackOnLand>(new Setting("On Land Attack", "attack-down-impact", Setting.ValueType.String));
                triggerDownAttack = true;
                return;
            }

            CallFeature<CombatAnimator>(new Setting("combatCondition", "attack-normal", Setting.ValueType.String));
        }
    }

    public void OnSpecialAttack(InputAction.CallbackContext context)
    {
        if (block) return;
        if (context.performed) CallFeature<CombatAnimator>(new Setting("combatCondition", "attack-special", Setting.ValueType.String));
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        if (context.performed) CallFeature<Block>(new Setting("toggleBlock", true, Setting.ValueType.Bool));
        if (context.canceled) CallFeature<Block>(new Setting("toggleBlock", false, Setting.ValueType.Bool));
    }
    public void CheckInputState()
    {
        if (GameManager.gameState == GameManager.GameState.Gameplay)
        {
            input.enabled = true;
        }
        else
        {
            input.enabled = false;
        }
    }
}
