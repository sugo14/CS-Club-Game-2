using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Bullet : MonoBehaviour
{
    BulletState bulletState;

    Rigidbody2D rb;


    void Awake()
    {
        // Play shoot sound effect
        SFXManager.Instance.PlayerShoot();

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
                SFXManager.Instance.BulletMiss();
                return;
            }

        }
        if (collision.gameObject.TryGetComponent<PlayerHandler>(out PlayerHandler playerHandler))
        {
            if (playerHandler.playerID == bulletState.ownerId)
            {
                // Ignore collision with the player who fired the bullet
                SFXManager.Instance.BulletMiss();
                return;
            }

            playerHandler.InflictDamage(bulletState.damage);

            // Play Hit Sound
            SFXManager.Instance.BulletHit();
        }
        else
        {
            // Play Miss Sound
            SFXManager.Instance.BulletMiss();
        }

        //Damage logic here

        // Destroy the bullet
        EntityRegistry.bullets.Remove(bulletState.id);

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
