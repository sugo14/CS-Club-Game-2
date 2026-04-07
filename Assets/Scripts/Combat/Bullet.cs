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
        ////Debug.Log($"Bullet collided with: {collision.gameObject.name}");
        if (collision.gameObject.TryGetComponent<PlayerHandler>(out PlayerHandler playerHandler))
        {

            playerHandler.InflictDamage(bulletState.damage);

            // Play Hit Sound
            SFXManager.Instance.BulletHit();
        }
        if (collision.gameObject.TryGetComponent<TowerHealth>(out TowerHealth towerHealth))
        {

            towerHealth.DammageTower(bulletState.damage);

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
        transform.localScale = new Vector3(bulletState.damage / 25f, bulletState.damage / 25f, 1f);

        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        if (rb != null)
        {
            rb.velocity = bulletState.velocity;
        }

        // Ignore physical collisions with GameObjects owned by the same player
        Collider2D myCollider = GetComponent<Collider2D>();
        if (myCollider != null)
        {
            // Ignore player collider(s)
            if (EntityRegistry.players.TryGetValue(state.ownerId, out GameObject playerObj) && playerObj != null)
            {
                foreach (Collider2D col in playerObj.GetComponentsInChildren<Collider2D>())
                {
                    Physics2D.IgnoreCollision(myCollider, col);
                }
            }

            // Ignore tower collider(s)
            if (EntityRegistry.towers.TryGetValue(state.ownerId, out GameObject towerObj) && towerObj != null)
            {
                foreach (Collider2D col in towerObj.GetComponentsInChildren<Collider2D>())
                {
                    Physics2D.IgnoreCollision(myCollider, col);
                }
            }

            // Ignore other bullets from the same player
            foreach (var kvp in EntityRegistry.bullets)
            {
                if (kvp.Value != null && kvp.Value != gameObject && 
                    kvp.Value.TryGetComponent<Bullet>(out Bullet otherBullet) && 
                    otherBullet.bulletState.ownerId == state.ownerId)
                {
                    if (kvp.Value.TryGetComponent<Collider2D>(out Collider2D otherCol))
                    {
                        Physics2D.IgnoreCollision(myCollider, otherCol);
                    }
                }
            }
        }
    }
}
