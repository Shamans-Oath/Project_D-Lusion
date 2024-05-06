using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Attack Settings")]
    public float attackCooldown = 1f;
    private float lastAttackTime;

    [Header("Fury Meter Settings")]
    [Range(0.0f,1.0f)]
    public float furymeterMax = 0f;
    [Range(0.0f, 1.0f)]
    public float furymeterGainRate = 0.05f;
    [Range(0.0f, 1.0f)]
    public float furymeterLossRate = 1f;
    public float currentFurymeter;

    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    private ActionControls inputs; // The generated class from the Input Actions asset

    public AttackController cmp_attackController;
    public PlayerMovement cmp_move;
    public Gravity cmp_gravity;
    public Jump cmp_jump;
    public LifeClass cmp_life;
    public PlayerDash cmp_dash;
    public PlayerRotation cmp_rotation;
    public Drag cmp_drag;
    public Slope cmp_slope;
    public LadderStep cmp_ladder;

    private Vector2 direction;
    private void Awake()
    {
        if (cmp_life == false && gameObject.GetComponent<LifeClass>()) cmp_life = gameObject.GetComponent<LifeClass>();
        if (cmp_jump == false && gameObject.GetComponent<Jump>()) cmp_jump = gameObject.GetComponent<Jump>();
        if (cmp_gravity == false && gameObject.GetComponent<Gravity>()) cmp_gravity = gameObject.GetComponent<Gravity>();
        if (cmp_move == false && gameObject.GetComponent<PlayerMovement>()) cmp_move = gameObject.GetComponent<PlayerMovement>();
        if (cmp_attackController == false && gameObject.GetComponent<AttackController>()) cmp_attackController = gameObject.GetComponent <AttackController>();
        if (cmp_dash == false && gameObject.GetComponent<PlayerDash>()) cmp_dash = gameObject.GetComponent<PlayerDash>();
        if (cmp_rotation == false && gameObject.GetComponent<PlayerRotation>()) cmp_rotation = gameObject.GetComponent<PlayerRotation>();
        if (cmp_drag == false && gameObject.GetComponent<Drag>()) cmp_drag = gameObject.GetComponent<Drag>();
        if (cmp_slope == false && gameObject.GetComponent<Slope>()) cmp_slope = gameObject.GetComponent<Slope>();
        if (cmp_ladder == false && gameObject.GetComponent<LadderStep>()) cmp_ladder = gameObject.GetComponent<LadderStep>();

        if (GameManager.gameInputSystem == null) GameManager.SetInputSystem();

        inputs = GameManager.gameInputSystem;
        currentHealth = maxHealth;
        currentFurymeter = furymeterMax;
    }

    private void OnEnable()
    {
        //Esto del cursos esta nomas por el testeo, hay que moverlo a otro sitio com el gamemanager o asi
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        if(cmp_attackController) cmp_attackController.cmp_hitBox.HitCheck += UpdateFurymeter;

        inputs.Enable();
        //habilitar el actions o como se llame que controle al player


        inputs.GamePlay.Move.performed += ctx => direction = ctx.ReadValue<Vector2>();
        inputs.GamePlay.Move.canceled += ctx => direction = ctx.ReadValue<Vector2>();
        //inputs.GamePlay.Move.performed += _ => Debug.Log("MoveInputDetected" + direction.x + " " + direction.y);
        inputs.GamePlay.Attack.performed+=_=>Attack();
        inputs.GamePlay.Dash.performed+=_=>Dash();
        //  inputs.GamePlay.SpecialAtack.performed += _ => attackController.SpecialAttack();

    }

    private void OnDisable()
    {
        if (cmp_attackController) cmp_attackController.cmp_hitBox.HitCheck -= UpdateFurymeter;
        //inputs.Disable();
        //deshabilitar el actions o como se llame que controle al player

        inputs.GamePlay.Move.performed -= ctx => direction = ctx.ReadValue<Vector2>();
        inputs.GamePlay.Move.canceled -= ctx => direction = ctx.ReadValue<Vector2>();
        //inputs.GamePlay.Move.performed -= _ => Debug.Log("MoveInputDetected" + direction.x + " " + direction.y);
        inputs.GamePlay.Attack.performed-=_=>Attack();
        inputs.GamePlay.Dash.performed-=_=>Dash();
        
        //inputs.GamePlay.SpecialAtack.performed -= _ => attackController.SpecialAttack();

    }

    private void Update()
    {

        //UpdateFurymeter();
        CheckHealthChanges();
        DashWaitLanding();
    }

    private void FixedUpdate()
    {
        cmp_ladder.OnStep(new Vector2(direction.y, direction.x));
        cmp_slope.OnSlope();

        if (!cmp_dash.isDashing && !cmp_ladder.onStep)
        {
            if (cmp_slope.onSlope) cmp_gravity.ApplyGravity(cmp_slope.slopeNormal, Time.fixedDeltaTime);
            else cmp_gravity.ApplyGravity(Time.fixedDeltaTime);
        }
        Move();

        cmp_gravity.LimitVerticalSpeed();
    }

    private void Move()
    {
        if (!cmp_dash.isDashing)
        {
            cmp_drag.ApplyDrag(new Vector2(direction.y, direction.x));
            cmp_move.LimitSpeed();
        }

        if (!cmp_gravity.isGrounded && !cmp_ladder.onStep) return;

        if (cmp_move.acceleration != 0 && direction != Vector2.zero)
        {
            cmp_move.Movement(direction.y, direction.x);
        }

        cmp_rotation.Rotate(new Vector2(direction.y, direction.x));
    }

    private void Attack()
    {
        if (Time.time > lastAttackTime + attackCooldown)
        {
            // Invoke attack function
            cmp_attackController.NormalAttack();
            lastAttackTime = Time.time;
        }
    }

    private void Dash()
    {
        if(!cmp_gravity.isGrounded) return;

        cmp_dash.ChargeDash(GameObject.Find("PuntoDash").transform.position);
    }

    private void DashWaitLanding()
    {
        cmp_dash.CheckLanding(cmp_gravity.isGrounded);
    }

    private void UpdateFurymeter()
    {
        IncreaseFurymeter(furymeterGainRate);
 
        //??? :v debería ser relacionado con los golpes, al dummy por ahora
        //BurneX: Ya se está llamando esta funcion con un action delegate (observer)
    }

    private void CheckHealthChanges()
    {

        //aiuda
        //FALTA
        //BurneX: LifeScript ahora tiene un action delegate que te puede servir para llamar funciones al ejecutarse cambios (Observer), puedes usarlo para esto
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if(currentHealth==0)
        {
            //RIP xD
        }
    }

    public void IncreaseFurymeter(float amount)
    {
        currentFurymeter = Mathf.Min(currentFurymeter + amount, furymeterMax);
    }

    public void DecreaseFurymeter(float amount)
    {
        currentFurymeter = Mathf.Max(currentFurymeter - amount, 0);
    }
}
