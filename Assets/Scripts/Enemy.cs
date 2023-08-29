using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    public float maxHealth; 
    private float currentHealth;

    public float speed = 1.5f;
    public float patrolDistance = 3f; // Distance in units the enemy will move left and right

    private float originalX;

    private Transform player;
    public float chaseRange = 5f; // Radius where enemy will start chasing
    public float chaseSpeed = 2.5f;

    private FireBehaviour fireBehaviourCache;
    private Animator animator;
    private void Start()
    {
        originalX = transform.position.x;
        animator = gameObject.GetComponent<Animator>();
    }

    private void Awake()
    {
        maxHealth = 4;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (player != null)
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
        else
        {
            Patrol();
        }
    }
    void ChasePlayer()
    {
        animator.SetFloat("speed", chaseSpeed);
        FlipSprite();
        // Move the enemy towards the player's position
        transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
    }

    void Patrol()
    {
        animator.SetFloat("speed", speed);
        FlipSprite();
        transform.position = new Vector2(originalX + Mathf.PingPong(Time.time * speed, patrolDistance * 2) - patrolDistance, transform.position.y);
    }

    private void FlipSprite()
    {
        bool playerIsLeftOfEnemy = player.position.x < transform.position.x;
        if ((playerIsLeftOfEnemy && transform.localScale.x > 0) || (!playerIsLeftOfEnemy && transform.localScale.x < 0))
        {
            Vector3 enemyScale = transform.localScale;
            enemyScale.x *= -1;
            transform.localScale = enemyScale;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Fire"))
        {
            if (fireBehaviourCache == null)
                fireBehaviourCache = collision.gameObject.GetComponent<FireBehaviour>();
            // Decrease health by 1/4
            currentHealth -= maxHealth / fireBehaviourCache.FireStrength;
            animator.SetTrigger("bullet");

            // If health goes below 0, handle enemy death.
            if (currentHealth <= 0)
            {
                gameObject.SetActive(false); // Destroy the enemy
                return; // Exit out of the function to avoid executing further code
            }

            // Calculate the direction away from the bullet
            Vector2 awayFromBullet = transform.position - collision.transform.position;
            // Push the enemy away
            rb.AddForce(awayFromBullet.normalized * fireBehaviourCache.PushStrength, ForceMode2D.Impulse);

            // Optionally, destroy the bullet after it hits the enemy
            Destroy(collision.gameObject);
        }
    }
}
