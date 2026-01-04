using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerGun : MonoBehaviour
{
    //TODO: Set this through code from the registery
    [SerializeField]
    int playerID = 1;

    //Gun stats
    [SerializeField]
    float reloadspeed = 2f; // seconds
    [SerializeField]
    int magazineSize = 3;


    //Fireing Stats
    [SerializeField]
    float spreadAngle = 0.08f; //degrees
    [SerializeField]
    float bulletSpeed = 5f;

    //Burst Stats
    [SerializeField]
    int bulletsPerBurst = 1;
    [SerializeField]
    int burstDelay = 50; //milliseconds

    //bullet Stats
    [SerializeField]
    float baseDamage = 10f;
    [SerializeField]
    float baseKnockback = 5f;

    //Special
    [SerializeField]
    float baseLifesteal = 0f;




    [SerializeField]
    private GameObject bulletPrefab;


    int nextBulletId = 000;

    bool isReloading = false;

    [SerializeField]
    int ammo;


    void Start()
    {
        ammo = magazineSize;
    }

    void Update()
    {

    }

    public void Fire()
    {
        StartCoroutine(FireBurstCoroutine());
    }

    IEnumerator FireBurstCoroutine()
    {
        for (int i = 0; i < bulletsPerBurst; i++)
        {
            if (ammo > 0)
            {
                CreateBullet();
            }
            else
            {
                Debug.Log("Out of Ammo!");

                Reload();

                yield break;
            }

            // Delay between bullets in a burst
            if (burstDelay > 0 && i < bulletsPerBurst - 1)
            {
                yield return new WaitForSeconds(burstDelay / 1000f);
            }
        }
    }

    public void Reload()
    {
        if (!isReloading)
        {
            Debug.Log("Reload");
            //Create a coroutine to handle reloading over time
            StartCoroutine(ReloadCoroutine());
            isReloading = true;
        }
    }

    // Create and initialize a new bullet
    GameObject CreateBullet()
    {

        //Debug.Log("CreateBullet");
        // Create a new bullet state
        BulletState newBulletState = new BulletState
        {
            id = playerID * 1000 + nextBulletId++,
            ownerId = playerID,
            //set this through code later
            damage = baseDamage,
            knockback = baseKnockback,
            lifestealPercent = baseLifesteal,


            velocity = bullet2DVelocity(bulletSpeed, transform.eulerAngles.z, spreadAngle),

        };

        // Instantiate the bullet prefab
        GameObject bulletGO = Instantiate(bulletPrefab, transform.position + transform.forward * 2, Quaternion.identity);

        if (bulletGO.TryGetComponent<Bullet>(out Bullet bulletComponent))
        {
            bulletComponent.setup(newBulletState);
        }
        else
        {
            Debug.LogError("Bullet prefab does not have a Bullet component.");
        }

        // Register the bullet in the EntityRegistry
        EntityRegistry.bullets.Add(newBulletState.id, bulletGO);

        ammo--;

        return bulletGO;
    }

    // Calculate 2D velocity vector from speed and angle in degrees
    Vector2 bullet2DVelocity(float speed, float angleDegrees, float spread)
    {
        angleDegrees += UnityEngine.Random.Range(-spread / 2, spread / 2);

        float angleRadians = angleDegrees * Mathf.Deg2Rad;
        float vx = speed * Mathf.Cos(angleRadians);
        float vy = speed * Mathf.Sin(angleRadians);
        return (new Vector2(vx, vy));
    }

    IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds(reloadspeed);
        Debug.Log("Reloaded");
        // Implement reload logic here (e.g., reset ammo count)
        ammo = magazineSize;
        isReloading = false;

    }


}
