using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    public float maxHealth; 
    private float currentHealth;

    private float pushStrength = 2.0f; // Strength at which the enemy is pushed away.

    private void Awake()
    {
        maxHealth = 4;
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Fire"))
        {
            // Decrease health by 1/4
            currentHealth -= maxHealth / collision.gameObject.GetComponent<FireBehaviour>().FireStrength;

            // If health goes below 0, handle enemy death.
            if (currentHealth <= 0)
            {
                Destroy(gameObject); // Destroy the enemy
                return; // Exit out of the function to avoid executing further code
            }

            // Calculate the direction away from the bullet
            Vector2 awayFromBullet = transform.position - collision.transform.position;

            // Push the enemy away
            rb.AddForce(awayFromBullet.normalized * collision.gameObject.GetComponent<FireBehaviour>().PushStrength, ForceMode2D.Impulse);

            // Optionally, destroy the bullet after it hits the enemy
            Destroy(collision.gameObject);
        }
    }
}
