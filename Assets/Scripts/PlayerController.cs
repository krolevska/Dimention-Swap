using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables

    [Header("Movement")]
    private float forwardInput;
    private float speed = 3.0f;
    private float jumpSpeed = 5.0f;
    private bool isOnGround;

    [Header("Shooting")]
    public GameObject firePrefab;
    public Transform firePoint;
    private int meleeDamage = 1;

    [Header("Health")]
    private int maxHealth = 20;
    private int currentHealth;

    [Header("PowerUps")]
    private float health;
    private bool hasGun;
    private bool hasKnife;
    private float armorMultiplier = 1f;  // Set to less than 1 if armor is active, e.g., 0.5 for 50% reduction

    [Header("States")]
    private bool normalDimension;
    private bool hasPowerUp;

    // Components
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    #endregion

    #region Unity Callbacks

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        ResetStates();
    }

    private void Update()
    {
        HandleInput();
        CheckPlayerHealth();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollisionEvents(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleTriggerEvents(collision);
    }

    #endregion

    #region Custom Methods

    private void ResetStates()
    {
        normalDimension = true;
        isOnGround = true;
        hasPowerUp = false;
        currentHealth = maxHealth;
    }

    private void HandleInput()
    {
        forwardInput = Input.GetAxis("Horizontal");
        HandleMovement();
        HandleActions();
    }

    private void HandleMovement()
    {
        if (forwardInput != 0)
        {
            animator.SetFloat("speed", Mathf.Abs(forwardInput));
            Run();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            animator.SetBool("jump", true);
            Jump();
        }
    }

    private void HandleActions()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Shoot();
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            animator.SetBool("fire", false);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            UseMeleeAttack();
        }
        else if (Input.GetKeyUp(KeyCode.R))
        {
            animator.SetBool("knife", false);
        }
    }

    private void CheckPlayerHealth()
    {
        if (currentHealth <= 0)
        {
            GameManager.instance.GameOver();
        }
    }

    private void Run()
    {
        transform.Translate(Vector3.right * forwardInput * speed * Time.deltaTime);
        spriteRenderer.flipX = forwardInput < 0;
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpSpeed, ForceMode2D.Impulse);
        isOnGround = false;
    }

    private void Shoot()
    {
        if (hasGun)
        {
            animator.SetBool("fire", true);
            GameObject fireInstance = Instantiate(firePrefab, firePoint.position, Quaternion.identity);

            float direction = spriteRenderer.flipX ? -1f : 1f;
            FireBehaviour fireBehaviour = fireInstance.GetComponent<FireBehaviour>();
            if (fireBehaviour != null)
            {
                fireBehaviour.SetDirection(direction);
            }
        }        
    }

    private void UseMeleeAttack()
    {
        if (hasKnife)
        {
            animator.SetBool("knife", true);
        }
    }

    private void HandleCollisionEvents(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            animator.SetBool("jump", false);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            ProcessEnemyCollision(collision.gameObject);
        }
    }

    private void HandleTriggerEvents(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Finish":
                GameManager.instance.EndLevel();
                break;
            case "Collectible":
                CollectItem(collision.gameObject);
                break;
            case "Powerup":
                PowerUp powerUpComponent = collision.gameObject.GetComponent<PowerUp>();
                if (powerUpComponent != null)
                {
                    CollectPowerUp(powerUpComponent.powerUpType);
                }
                Destroy(collision.gameObject);
                break;
        }
    }


    private void ProcessEnemyCollision(GameObject enemy)
    {
        if (Input.GetKey(KeyCode.R))
        {
            enemy.GetComponent<Enemy>().TakeDamage(meleeDamage);
        }
        else
        {
            animator.SetTrigger("hurt");
            TakeDamage(enemy.GetComponent<Enemy>().enemyDamage);
        }
    }


    private void CollectItem(GameObject item)
    {
        ScoreManager.instance.AddScore(1);
        Destroy(item);
    }

    // Collect PowerUp
    public void CollectPowerUp(PowerUpType powerUpType)
    {
        switch (powerUpType)
        {
            case PowerUpType.Armor:
                armorMultiplier = 0.5f;
                break;
            case PowerUpType.Drug:
                currentHealth = Mathf.Min(currentHealth + 20, maxHealth); // Ensure health doesn't exceed the max
                break;
            case PowerUpType.Gun:
                hasGun = true;
                break;
            case PowerUpType.Knife:
                hasKnife = true;
                meleeDamage = 5;
                break;
            default:
                break;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= (int)(damage * armorMultiplier);  // Use currentHealth here, and convert to int if damage isn't an integer
        CheckPlayerHealth();  // Check player's health immediately after taking damage
    }
    #endregion
}
