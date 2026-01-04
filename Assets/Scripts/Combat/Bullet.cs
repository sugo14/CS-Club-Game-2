using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Bullet : MonoBehaviour
{
    BulletState bulletState;

    Rigidbody2D rb;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        //rotate sprite to velocity direction
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {


        //Debug.Log($"Bullet hit: {collision.gameObject.name}");
        if (collision.gameObject.TryGetComponent<Bullet>(out Bullet hitBullet))
        {
            if (bulletState.ownerId == hitBullet.bulletState.ownerId)
            {
                // Ignore collision with bullets from the same owner
                return;
            }
        }
            
        //Damage logic here


        // Destroy the bullet
        Destroy(gameObject);

    }

    public void setup(BulletState state)
    {
        bulletState = state;
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        if (rb != null)
        {
            rb.velocity = bulletState.velocity;
        }
    }
}
