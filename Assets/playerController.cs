using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Attack Settings")]
    public float attackCooldown = 1f;
    private float lastAttackTime;

    [Header("Fury Meter Settings")]
    public float furymeterMax = 100f;
    public float furymeterGainRate = 5f;
    public float furymeterLossRate = 100f;
    private float currentFurymeter;

    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    private ActionControls inputs; // The generated class from the Input Actions asset

    [SerializeField] private AttackController attackController;

    [SerializeField] private movementTest movementTest;


    private void Awake()
    {
        if (GameManager.gameInputSystem == null) GameManager.SetInputSystem();

        inputs = GameManager.gameInputSystem;
        currentHealth = maxHealth;
        currentFurymeter = furymeterMax;
    }

    private void OnEnable()
    {
        inputs.Enable();
        //habilitar el actions o como se llame que controle al player
        inputs.GamePlay.Attack.performed+=_=>Attack();
        //inputs.GamePlay.SpecialAtack.performed += _ => attackController.SpecialAttack();
    }

    private void OnDisable()
    {
        //inputs.Disable();
        //deshabilitar el actions o como se llame que controle al player
        inputs.GamePlay.Attack.performed-=_=>Attack();
        //inputs.GamePlay.SpecialAtack.performed -= _ => attackController.SpecialAttack();

    }

    private void Update()
    {
        Move();
        UpdateFurymeter();
        CheckHealthChanges();
    }

    private void Move()
    {
       //movementTest.
    }

    private void Attack()
    {
        if (Time.time > lastAttackTime + attackCooldown)
        {
            // Invoke attack function
            attackController.NormalAttack();
            lastAttackTime = Time.time;
        }
    }



    private void UpdateFurymeter()
    {
        //??? :v debería ser relacionado con los golpes, al dummy por ahora
    }

    private void CheckHealthChanges()
    {
        //aiuda
        //FALTA
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if(currentHealth==0)
        {
            //RIP
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
