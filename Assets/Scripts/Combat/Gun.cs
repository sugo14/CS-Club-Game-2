using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gun : MonoBehaviour
{
    //TODO: Set this through code from the registery
    [SerializeField]
    int playerID = 1;

    //Gun stats
    [SerializeField]
    float reloadspeed = 2f; // seconds
    [SerializeField]
    int magazineSize = 3;
    [SerializeField]
    float fireDelay = 0.5f; // seconds

    //Firing Stats
    [SerializeField]
    float spreadAngle = 0.08f; //degrees
    [SerializeField]
    float bulletSpeed = 10f;
    
    //Burst Stats
    [SerializeField]
    int bulletsPerBurst = 1;
    [SerializeField]
    float burstDelay = 50; //milliseconds

    //bullet Stats
    [SerializeField]
    float baseDamage = 10f;
    [SerializeField]
    float baseKnockback = 5f;

    //Special
    [SerializeField]
    float baseLifesteal = 0f;

    [SerializeField]
    float targetingRange = 5f;


    [SerializeField]
    bool turretMode = false;

    [SerializeField]
    private GameObject bulletPrefab;


    int nextBulletId = 000;

    bool isReloading = false;
    float lastFireTime = -Mathf.Infinity; // Track when we last fired

    [SerializeField]
    int ammo;

    CircleCollider2D targetAreaCol;


    void Start()
    {
        ammo = magazineSize;
        targetAreaCol = GetComponent<CircleCollider2D>();
        targetAreaCol.radius = targetingRange;
        targetAreaCol.isTrigger = true; // Make sure it's a trigger

    }

    void Update()
    {
        if (turretMode)
        {
            Transform targetTrans = FindClosestVisibleTarget(
                transform.position,
                new ContactFilter2D
                {
                    useLayerMask = true,
                    layerMask = LayerMask.GetMask("Player"),
                    useTriggers = false
                },
                LayerMask.GetMask("Solid", "Player")
            );

            if (targetTrans != null)
            {
                Vector2 direction = (targetTrans.position - transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
                
                // Only fire if enough time has passed since last fire
                if (Time.time - lastFireTime >= fireDelay)
                {
                    Fire();
                    
                }
            }
        }
    }

    public void Fire()
    {
        if (Time.time - lastFireTime >= fireDelay)
        {
            lastFireTime = Time.time;
            StartCoroutine(FireBurstCoroutine());

        }

        
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
            StartCoroutine(ReloadCoroutine());
            isReloading = true;
        }
    }

    // Create and initialize a new bullet
    GameObject CreateBullet()
    {
        //Replace with code to create bullet state from registry once players implemeneted
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
        // Implement reload logic here (e.g., reset ammo count)
        ammo = magazineSize;
        isReloading = false;

    }

    public Transform FindClosestVisibleTarget(
    Vector2 origin,
    ContactFilter2D contactFilter,
    LayerMask obstacleMask)
    {
        List<Collider2D> hits = new List<Collider2D>();
        int count = targetAreaCol.Overlap(contactFilter, hits);
        
        Transform closest = null;
        float closestDist = float.PositiveInfinity;

        foreach (var hit in hits)
        {
            Vector2 dir = (hit.bounds.center - (Vector3)origin);
            float dist = dir.magnitude;

            RaycastHit2D los = Physics2D.Raycast(
                origin,
                dir.normalized,
                dist,
                obstacleMask
            );

            // If the first thing we hit is the target, it's visible
            if (los.collider == hit)
            {
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closest = hit.transform;
                }
            }
        }

        return closest;
    }



}
