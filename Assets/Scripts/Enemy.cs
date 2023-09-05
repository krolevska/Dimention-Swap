using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Variables

    [Header("Properties")]
    public float health = 20;
    public int enemyDamage = 5;
    private float currentHealth;

    [Header("Movement")]
    public float speed = 1.5f;
    public float patrolDistance = 3f;
    private float originalX;

    [Header("Chase Player")]
    public float chaseRange = 5f;
    public float chaseSpeed = 2.5f;
    private Transform player;


    // Components
    private Rigidbody2D rb;
    private Animator animator;
    private FireBehaviour fireBehaviourCache;

    #endregion

    #region Unity Callbacks

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = health;
    }

    private void Start()
    {
        originalX = transform.position.x;
    }

    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < chaseRange)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Fire"))
        {
            ProcessFireCollision(collision);
        }
    }

    #endregion

    #region Custom Methods

    private void ChasePlayer()
    {
        animator.SetFloat("speed", chaseSpeed);
        FlipSpriteTowardsPlayer();
        transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
    }

    private void Patrol()
    {
        animator.SetFloat("speed", speed);
        transform.position = new Vector2(originalX + Mathf.PingPong(Time.time * speed, patrolDistance * 2) - patrolDistance, transform.position.y);
    }

    private void FlipSpriteTowardsPlayer()
    {
        bool playerIsLeftOfEnemy = player.position.x < transform.position.x;
        Vector3 enemyScale = transform.localScale;

        enemyScale.x = playerIsLeftOfEnemy ? -Mathf.Abs(enemyScale.x) : Mathf.Abs(enemyScale.x);
        transform.localScale = enemyScale;
    }

    private void ProcessFireCollision(Collision2D collision)
    {
        if (fireBehaviourCache == null)
        {
            fireBehaviourCache = collision.gameObject.GetComponent<FireBehaviour>();
        }

        currentHealth -= fireBehaviourCache.FireStrength;
        animator.SetTrigger("bullet");

        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
            return;
        }

        Vector2 awayFromBullet = transform.position - collision.transform.position;
        rb.AddForce(awayFromBullet.normalized * fireBehaviourCache.PushStrength, ForceMode2D.Impulse);

        Destroy(collision.gameObject);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    #endregion
}
