using Features;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

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
        RaizTotal,
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
    public float lastAttackTime = 0f;
    public bool specialAttack= false;

    [Header("Movement")]
    public float minDistance = 2.5f;
    public float maxDistance = 5f;
    public float chaseMoveSpeed = 5f;
    private NavMeshAgent agent;
    public float stoppingDistance = 0.1f;
    public float rotationSpeed=5f;

    [Header("Embestida")]
    public GameObject pointPrefab;
    public int numPositions = 4;            
    public float spawnRadius = 5f;          
    public float moveDelay = 2f;           
    public float moveSpeed = 5f;
    public List<Transform> spawnedPositions = new List<Transform>();
    private bool isMoving = false;
    public float chargeSpeed = 40f;
    public float chargeDelay = 1f;
    private Vector3 chargeTargetPosition;
    private bool isCharging = false;
    public float chargeHomingFactor = 0.1f;
    public float chargeCooldown = 20f;

    [Header("Minions")]
    public GameObject enemyPrefab;
    public int numEnemies = 5;
    public float enemySpawnRadius = 10f;
    public LayerMask groundLayer;
    public float spawnDelay = 5f;
    public float minionCooldown = 30f;

    [Header("Raiz")]
    public GameObject raizPrefab;
    public float raizCastTime;
    public float raizCooldown = 20f;

    [Header("Raiz Total")]
    public GameObject raizTotalPrefab;
    public float raizTotalCastTime;

    [Header("ClonePhase")]
    public GameObject objectToDeactivate; 
    public GameObject chargingPrefab;   
    public GameObject[] shadowSpawnPoints;        
    public float shadowChargeSpeed = 10f;
    public float shadowChargeDelay = 3f;
    public List<GameObject> spawnedObjects = new List<GameObject>();
    public int currentChargerIndex = -1;
    [Header("Materials")]
    public GameObject cube;
    public Material materialToFade;
    public float fadeDuration = 0.5f;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        currentState = BossState.Idle;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        shadowSpawnPoints = GameObject.FindGameObjectsWithTag("Shadow");
        cube.GetComponent<MeshRenderer>().material = materialToFade;
        
    }

    // Update is called once per frame
    void Update()
    {

        if (specialAttack) 
        {
            agent.enabled = false;
            return; 
        }
        else
        {
            agent.enabled = true;
        }
        CooldownTimers();
        switch (currentState)
            {
                case BossState.Idle:
                    if (Time.time - lastAttackTime > attackCooldown)
                    {
                        ChooseAttack();
                        FollowPlayer();
                    }
                    if (health <= enrageThreshold * 100f / health)
                    {
                        currentState = BossState.Enraged;
                        //animator.SetTrigger("Enrage");

                    }
                    if(health<= 50)
                    {
                    currentState = BossState.RaizTotal;
                    }
                    break;

                case BossState.Attack:
                //animator.SetTrigger("Attack"); 
                Debug.Log("Te ataco lol");
                    lastAttackTime = Time.time;
                    currentState = BossState.Idle;
                    break;

                case BossState.DefensiveDash:
                    //animator.SetTrigger("Dash");
                    lastAttackTime = Time.time;
                    Dash();
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
                    if (Time.time - lastAttackTime > attackCooldown && raizCooldown <=0.1f)
                    {
                    Raiz();
                    }
                    else
                    {
                        ChooseAttack();
                    }
                    break;
            case BossState.ChargeAttack:
                if (Time.time - lastAttackTime > attackCooldown && chargeCooldown <= 0.1f)
                {
                    ChargeAttack();
                }
                else
                {
                    ChooseAttack();
                }
                break;

            case BossState.EnemySpawn:
                if (Time.time - lastAttackTime > attackCooldown && minionCooldown <= 0.1f)
                {
                    SpawnChamos();
                }
                {
                    ChooseAttack();
                }
                break;
            case BossState.RaizTotal:
                if (Time.time - lastAttackTime > attackCooldown)
                {
                    RaizTotal();
                }
                break;
        }
    }
    public void FadeOut()
    {
        StartCoroutine(FadeCoroutine(1f, -1f));

    }


    public void FadeIn()

    {

        StartCoroutine(FadeCoroutine(0f, 1f));

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player" && dashCooldown <= 0)
        {
            Dash();
        }

    }
    private void FollowPlayer()
    {
        if (!specialAttack)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);



            if (distanceToPlayer < minDistance || distanceToPlayer > maxDistance)

            {
                Vector3 directionToPlayer = (player.position - transform.position).normalized;

                Vector3 targetPosition = player.position - directionToPlayer * Mathf.Clamp(distanceToPlayer, minDistance, maxDistance);

                agent.SetDestination(targetPosition);

                agent.stoppingDistance = stoppingDistance;
                //rotation stuff
                agent.isStopped = false;
                Vector3 targetForward = new Vector3(directionToPlayer.x, 0, directionToPlayer.z);
                Quaternion targetRotation = Quaternion.LookRotation(targetForward, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            }


            else

            {
                agent.isStopped = true;
            }
            

        }
    }
    public void Dash()
    {
        if (!isDashing && !specialAttack)
        {
            StartCoroutine(DefensiveDashCoroutine());
        }
    }
    public void CooldownTimers()
    {
        if (dashCooldown >= 0)
            dashCooldown -= Time.deltaTime;
        if (specialAttack)
            return;
        if(chargeCooldown >= 0)
           chargeCooldown -= Time.deltaTime;
        if(raizCooldown >= 0)
           raizCooldown -= Time.deltaTime;
        if (minionCooldown >= 0)
            minionCooldown -= Time.deltaTime;

    }

    private void Raiz()
    {
        specialAttack = true;
        StartCoroutine(RaizSpecial());
    }
    private IEnumerator RaizSpecial()
    {
       
        yield return new WaitForSeconds(raizCastTime);
        Instantiate(raizPrefab, transform.position, Quaternion.identity);
        currentState = BossState.Idle;
        specialAttack = false;
        raizCooldown = 20f;
    }
    private void RaizTotal()
    {
        specialAttack = true;
        StartCoroutine(RaizTotalSpecial());
    }
    private IEnumerator RaizTotalSpecial()
    {

        yield return new WaitForSeconds(raizCastTime);
        Instantiate(raizTotalPrefab, player.position, Quaternion.identity);
        currentState = BossState.Idle;
        specialAttack = false;
        //lastAttackTime = 20f;
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

        int attackChoice = Random.Range(1, 5);

        switch (attackChoice)
        {
            case 1:
                currentState = BossState.Attack;
                break;
            case 2:
                currentState = BossState.Raiz;
                break;
            case 3:
                currentState = BossState.EnemySpawn;
                break;
            case 4:
                currentState = BossState.ChargeAttack;
                break;
        }
        lastAttackTime = 5f;
    }
   public void ChargeAttack()
    {
        specialAttack = true;
        //animator.SetTrigger("ChargeAttack");
        //apagar el renderer TO-DO
        StartCoroutine(SpawnAndMove());

    }
    public void SpawnChamos()
    {
        specialAttack = true;
        //animator.SetTrigger("ChargeAttack");
        StartCoroutine(SpawnEnemiesCoroutine());
    }
    private System.Collections.IEnumerator SpawnAndMove()
    {
        spawnedPositions.Clear();
        yield return new WaitForSeconds(moveDelay);
        FadeOut();
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
        gameObject.transform.parent = targetPosition;
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
        gameObject.transform.parent = null;
        FadeIn();
        chargeTargetPosition = player.position;
        //homing
        while (Vector3.Distance(transform.position, player.position) > 0.5f)
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
        chargeCooldown = 20;
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
            currentState = BossState.Idle;
            specialAttack = false;
        }
        minionCooldown = 30;
    }
    private IEnumerator FadeCoroutine(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;
        Color currentColor = materialToFade.color;
        while (elapsedTime < fadeDuration)
        {
            float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);

            currentColor.a = currentAlpha;
            materialToFade.color = currentColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        currentColor.a = endAlpha;
        materialToFade.color = currentColor;
    }

}
