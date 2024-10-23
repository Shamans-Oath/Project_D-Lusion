using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Dash")]
    public Transform player;
    public float dashDistance = 3f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 0f;
    private bool isDashing = false;
    private float dashTimer = 20f;
    public enum BossState
    {
        Idle,
        Attack,
        Raiz,
        DefensiveDash,
        OffensiveDash,
        Enraged,
        Channeling,
        ChargeAttack,
        EnemySpawn,
    }
    [Header("Stats")]
    public BossState currentState;
    public float health = 100f;
    public float enrageThreshold = 25f;
    private Animator animator;
    public float attackCooldown = 4f;
    private float lastAttackTime = 0f;
    public bool specialAttack= false;

    [Header("Embestida")]
    public GameObject pointPrefab;
    public int numPositions = 4;            
    public float spawnRadius = 5f;          
    public float moveDelay = 2f;           
    public float moveSpeed = 5f;
    private List<Transform> spawnedPositions = new List<Transform>();
    private bool isMoving = false;
    public float chargeSpeed = 40f;
    public float chargeDelay = 1f;
    private Vector3 chargeTargetPosition;
    private bool isCharging = false;
    public float chargeHomingFactor = 0.1f;

    [Header("Minions")]
    public GameObject enemyPrefab;
    public int numEnemies = 5;
    public float enemySpawnRadius = 10f;
    public LayerMask groundLayer;
    public float spawnDelay = 0.5f;
    void Start()
    {
        currentState = BossState.Idle;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        DashCooldownTimer();
        if(specialAttack) { return; }
            switch (currentState)
            {
                case BossState.Idle:
                    if (Time.time - lastAttackTime > attackCooldown)
                    {
                        ChooseAttack();
                    }
                    if (health <= enrageThreshold * 100f / health)
                    {
                        currentState = BossState.Enraged;
                        //animator.SetTrigger("Enrage");

                    }
                    break;

                case BossState.Attack:
                    //animator.SetTrigger("Attack"); 
                    lastAttackTime = Time.time;
                    currentState = BossState.Idle;
                    break;

                case BossState.DefensiveDash:
                    //animator.SetTrigger("Dash");
                    lastAttackTime = Time.time;
                    //Dash();
                    currentState = BossState.Idle;
                    break;


                case BossState.OffensiveDash:
                    //animator.SetTrigger("Dash2");
                    lastAttackTime = Time.time;
                    currentState = BossState.Idle;
                    break;

                case BossState.Enraged:


                    attackCooldown = 1f;

                    if (Time.time - lastAttackTime > attackCooldown)
                    {
                        ChooseAttack();
                    }
                    break;
                case BossState.Raiz:
                    if (Time.time - lastAttackTime > attackCooldown)
                    {
                        ChooseAttack();
                    }
                    break;
            case BossState.ChargeAttack:
                if (Time.time - lastAttackTime > attackCooldown)
                {
                    ChargeAttack();
                }
                break;

            case BossState.EnemySpawn:
                if (Time.time - lastAttackTime > attackCooldown)
                {
                    ChooseAttack();
                }
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player" && dashCooldown <= 0)
        {
            Dash();
        }

    }
    public void Dash()
    {
        if (!isDashing && !specialAttack)
        {
            StartCoroutine(DefensiveDashCoroutine());
        }
    }
    public void DashCooldownTimer()
    {
        if (dashCooldown >= 0)
            dashCooldown -= Time.deltaTime;
    }


    System.Collections.IEnumerator DefensiveDashCoroutine()
    {
        isDashing = true;

        Vector3 dashDirection = (transform.position - player.position).normalized;
        Vector3 targetPosition = transform.position + dashDirection * dashDistance;
        Vector3 startPosition = transform.position;

        float elapsedTime = 0f;
        while (elapsedTime < dashDuration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / dashDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;

        isDashing = false;
        dashCooldown = dashTimer;
    }
    System.Collections.IEnumerator OfensiveDashCoroutine()
    {
        isDashing = true;

        Vector3 dashDirection = (player.position - transform.position).normalized;
        Vector3 targetPosition = transform.position + dashDirection * dashDistance;
        Vector3 startPosition = transform.position;

        float elapsedTime = 0f;
        while (elapsedTime < dashDuration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / dashDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;

        isDashing = false;
        dashCooldown = dashTimer;
    }
    void ChooseAttack()
    {

        int attackChoice = Random.Range(1, 4);

        switch (attackChoice)
        {
            case 1:
                currentState = BossState.Attack;
                break;
            case 2:
                currentState = BossState.Raiz;
                break;
            case 3:
                currentState = BossState.DefensiveDash;
                break;
        }
    }
   public void ChargeAttack()
    {
        specialAttack = true;
        //animator.SetTrigger("ChargeAttack");
        //apagar el renderer TODO
        StartCoroutine(SpawnAndMove());

    }
    private System.Collections.IEnumerator SpawnAndMove()
    {
        
        yield return new WaitForSeconds(moveDelay);
        SpawnPositions();
        if (!isMoving && spawnedPositions.Count > 0)
        {
            StartCoroutine(MoveToRandomPosition());
        }
    }
    void SpawnPositions()
    {
        for (int i = 0; i < numPositions; i++)
        {
            float angle = i * 2f * Mathf.PI / numPositions;
            Vector3 positionOffset = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * spawnRadius;
            Vector3 spawnPosition = transform.position + positionOffset;
            GameObject point = Instantiate(pointPrefab, spawnPosition, Quaternion.identity);
            spawnedPositions.Add(point.transform);
        }
    }
    private System.Collections.IEnumerator MoveToRandomPosition()
    {
        isMoving = true;
        Transform targetPosition = spawnedPositions[Random.Range(0, spawnedPositions.Count)];
        while (Vector3.Distance(transform.position, targetPosition.position) > 0.1f)
        {

            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, 200f * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPosition.position;
        isMoving = false;
        StartCharge();
    }
    public void StartCharge()
    {
        if (!isCharging && player != null) 
        {
            StartCoroutine(ChargeCoroutine());
        }
        else if (player == null)
        {
            Debug.LogError("No player");
        }
    }
    private System.Collections.IEnumerator ChargeCoroutine()
    {
        isCharging = true;
        
        yield return new WaitForSeconds(chargeDelay);
        chargeTargetPosition = player.position;
        //homing
        while (Vector3.Distance(transform.position, player.position) > 1.1f)
        {
            Vector3 directionToTarget = (chargeTargetPosition - transform.position).normalized;
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            Vector3 curvedDirection = Vector3.Lerp(directionToTarget, directionToPlayer, chargeHomingFactor);
            transform.position += curvedDirection * chargeSpeed * Time.deltaTime;
            yield return null;
        }
        //transform.position = chargeTargetPosition;
        isCharging = false;
        currentState = BossState.Idle;
        specialAttack = false;
        dashCooldown = 20;
    }

private System.Collections.IEnumerator SpawnEnemiesCoroutine()
    {
        yield return new WaitForSeconds(spawnDelay);
        for (int i = 0; i < numEnemies; i++)
        {
            Vector2 randomCirclePoint = Random.insideUnitCircle * enemySpawnRadius;
            Vector3 spawnPosition = transform.position + new Vector3(randomCirclePoint.x, 0f, randomCirclePoint.y);
            if (groundLayer != 0)
            {

                if (Physics.Raycast(spawnPosition + Vector3.up * 5f, Vector3.down, out RaycastHit hit, 10f, groundLayer))
                {
                    spawnPosition = hit.point;
                }

            }
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

}
