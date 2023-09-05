using UnityEngine;

public class FireBehaviour : MonoBehaviour
{
    public float Speed { get; } = 15.0f;

    private float aliveTimer = 5.0f;
    private float fireStrength = 4.0f;
    private float pushStrength = 2.0f;

    public float FireStrength => fireStrength;
    public float PushStrength => pushStrength;

    public enum BulletDirection
    {
        LEFT = -1,
        RIGHT = 1
    }
    private BulletDirection direction = BulletDirection.RIGHT;

    private void Update()
    {
        Vector2 moveDirection = new Vector2((int)direction, 0);
        transform.position += new Vector3(moveDirection.x, moveDirection.y, 0) * Speed * Time.deltaTime;
    }

    public void SetDirection(float dir)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (dir > 0)
        {
            direction = BulletDirection.RIGHT;
            spriteRenderer.flipX = false;
        }
        else
        {
            direction = BulletDirection.LEFT;
            spriteRenderer.flipX = true;
        }
    }

    public void InitiateSelfDestruction()
    {
        Destroy(gameObject, aliveTimer);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        InitiateSelfDestruction();
    }
}

public static class Utility
{
    public static T FindAnyObjectByType<T>() where T : Object
    {
        return GameObject.FindObjectOfType<T>();
    }
}
