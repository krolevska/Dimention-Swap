using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables

    [Header("Movement")]
    private float forwardInput;
    private float verticalInput;
    private float speed = 3.0f;
    private float jumpSpeed = 7.0f;
    private bool isOnGround;
    private bool isClimbing;

    [Header("Shooting")]
    public GameObject firePrefab;
    public Transform firePoint;

    [Header("Health")]
    private int maxHealth = 20;
    private int currentHealth;

    [Header("PowerUps")]
    private float health;
    private bool hasGun;
    private bool hasKnife;
    private float armorMultiplier = 1f;  // Set to less than 1 if armor is active, e.g., 0.5 for 50% reduction

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
        Debug.Log("Trigger entered with tag: " + collision.gameObject.tag);
        HandleTriggerEvents(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Stairs"))
        {
            isClimbing = false;
            rb.gravityScale = 1;
            animator.SetBool("isClimbing", false); // You might need to add this in the Animator if you want a different behavior when exiting stairs.
        }
    }


    #endregion

    #region Custom Methods

    private void ResetStates()
    {
        isOnGround = true;
        isClimbing = false;
        currentHealth = maxHealth;
    }

    private void HandleInput()
    {
        forwardInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
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

        if (verticalInput != 0 && isClimbing)
        {
            animator.SetFloat("climbingSpeed", Mathf.Abs(verticalInput));
            Climb();
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
    }

    private void CheckPlayerHealth()
    {
        if (currentHealth <= 0)
        {
            GameManager.instance.GameOver();
        }
    }

    private void Climb()
    {
        transform.Translate(Vector3.up * verticalInput * speed * Time.deltaTime);
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
            case "Stairs":
                Debug.Log("Entered Stairs Trigger");
                animator.SetBool("isClimbing", true);
                isClimbing = true;
                rb.gravityScale = 0;
                break;
        }
    }


    private void ProcessEnemyCollision(GameObject enemy)
    {
            animator.SetTrigger("hurt");
            TakeDamage(enemy.GetComponent<Enemy>().enemyDamage);
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
            default:
                break;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= (int)(damage * armorMultiplier);  // Use currentHealth here, and convert to int if damage isn't an integer
        CheckPlayerHealth();  // Check player's health immediately after taking damage
        ScoreManager.instance.DecreaseHealth(damage);
    }
    #endregion
}
