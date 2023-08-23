using UnityEngine;

public class FireBehaviour : MonoBehaviour
{
    public float speed = 15.0f;
    private float aliveTimer = 5.0f;
    private float fireStrength = 4.0f;
    private float pushStrength = 2.0f;

    public float FireStrength { get { return fireStrength; } }
    public float PushStrength { get { return pushStrength; } }
    public enum BulletDirection
    {
        LEFT = -1,
        RIGHT = 1
    }
    private BulletDirection direction = BulletDirection.RIGHT;

    public void SetDirection(float dir)
    {
        if (dir > 0)
        {
            direction = BulletDirection.RIGHT;
        }
        else
        {
            direction = BulletDirection.LEFT;
        }
    }

    void Update()
    {
        Vector3 moveDirection = new Vector3((int)direction, 0, 0);
        transform.position += moveDirection * speed * Time.deltaTime;
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
