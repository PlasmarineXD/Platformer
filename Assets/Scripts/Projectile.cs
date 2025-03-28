using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;
    public float Damage = 2.0f;
    public float LifeTime = 2.0f;

    private float LifeTimer;

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        LifeTimer = LifeTime;
    }

    private void Update()
    {
        LifeTime -= Time.deltaTime;
        if (LifeTime < 0)
        {
            Destroy(gameObject);
        }
        if (transform.position.magnitude > 100f)
        {
            Destroy(gameObject);
        }
    }

    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Base_Enemy enemy = other.GetComponent<Base_Enemy>();
        if (enemy != null)
        {
            enemy.GetComponent<HealthSystem>().TakeDamage(Damage);
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}
