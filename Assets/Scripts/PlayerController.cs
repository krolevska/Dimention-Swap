using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool normalDimension;
    private float forwardInput;
    private float speed = 3.0f;
    private float jumpSpeed = 5.0f;
    private bool isOnGround;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isInShadowCoroutine = false;  // Flag to check if coroutine is active

    public GameObject firePrefab;  // Drag your bullet (fire) prefab here in the inspector
    public Transform firePoint;    // Point from where the bullet is fired

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        normalDimension = true;
        isOnGround = true;
    }

    void Update()
    {
        // Detect horizontal input
        forwardInput = Input.GetAxis("Horizontal");

        if (forwardInput != 0)
        {
            Run();
        }

        // Detect jump input
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            isOnGround = false;
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            SwapDimension();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Shoot();
        }

    }

    private void Run()
    {
        transform.Translate(Vector3.right * forwardInput * speed * Time.deltaTime);

        // Flip the player sprite based on movement direction
        spriteRenderer.flipX = forwardInput < 0;
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpSpeed, ForceMode2D.Impulse);
        isOnGround = false;
    }
    private void SwapDimension()
    {
        if (normalDimension) // Switching to shadow dimension
        {
            normalDimension = false;
            Debug.Log("Switched to shadow dimension at: " + Time.time);

            // If not already in the shadow coroutine, start it
            if (!isInShadowCoroutine)
            {
                isInShadowCoroutine = true;
                StartCoroutine(CountTimeInShadowDimension());
            }
        }
        else // Switching to normal dimension
        {
            normalDimension = true;
            Debug.Log("Switched to normal dimension at: " + Time.time);

            // If returning from shadow dimension, stop the coroutine
            if (isInShadowCoroutine)
            {
                isInShadowCoroutine = false;
                StopCoroutine(CountTimeInShadowDimension());
            }
        }
    }
    private IEnumerator CountTimeInShadowDimension()
    {
        // Wait for 10 seconds in the shadow dimension
        yield return new WaitForSeconds(10f);
        isInShadowCoroutine = false; // Reset the flag when coroutine ends
        SwapDimension(); // Switch back to the normal dimension
    }
    private void Hide()
    {
        // Implement hide mechanic
    }

    private void Shoot()
    {
        // Instantiate the bullet
        GameObject fireInstance = Instantiate(firePrefab, firePoint.position, Quaternion.identity);

        // The direction is based on the way the character sprite is facing.
        float direction = spriteRenderer.flipX ? -1f : 1f;    

        // Set the bullet's direction based on player's facing direction
        FireBehaviour fireBehaviour = fireInstance.GetComponent<FireBehaviour>();
        if (fireBehaviour != null)
        {
            fireBehaviour.SetDirection(direction);
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
        // When in the "normal dimension", the player can interact with (stand on, collide with) "normal" platforms and pass through "shadow" platforms.
        if (normalDimension && collision.gameObject.CompareTag("Normal Enemy"))
        {
            // Handle interaction with normal enemies when in normal dimension.
            HandleNormalEnemyInteraction();
        }
        else if (!normalDimension && collision.gameObject.CompareTag("Shadow Enemy"))
        {
            // Handle interaction with shadow enemies when in shadow dimension.
            HandleShadowEnemyInteraction();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("End Point"))
        {
            // Trigger end of level events or transitions.
            EndLevel();
        }
        else if (collision.gameObject.CompareTag("Collectible"))
        {
            // Increase player's score or inventory.
            CollectItem(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Powerup"))
        {
            // Boost the player's abilities or provide some benefits.
            ActivatePowerup(collision.gameObject);
        }
    }
    private void HandleNormalEnemyInteraction()
    {
        // Logic for when the player collides with a normal enemy in the normal dimension.
        // Reduce player health.
    }
    private void HandleShadowEnemyInteraction()
    {
        // Logic for when the player collides with a shadow enemy in the shadow dimension.
        // Reduce player health.
    }
    private void CollectItem(GameObject item)
    {
        // Logic to handle the collection of items.
        // Deactivate the collected item so it disappears from the scene.
        item.SetActive(false);

        // When collected necessary amount of items for a level, call EndLevel()
    }
    private void ActivatePowerup(GameObject powerup)
    {
        // Logic to activate a power-up.
        // Deactivate the powerup item so it disappears.
        powerup.SetActive(false);
    }
    private void EndLevel()
    {
        // Transition to the next level and display a "level completed" message.
        // Store the player's score and other relevant stats.
    }
}