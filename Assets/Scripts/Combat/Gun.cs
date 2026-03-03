using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gun : MonoBehaviour
{
    [NonSerialized] public int playerID; // assigned on instantiation in GameStateHandler
    [NonSerialized] public PlayerState playerState;

    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    bool turretMode = false;

    public GameObject player;

    int nextBulletId = 0;

    bool isReloading = false;
    float lastFireTime = -Mathf.Infinity;

    int ammo;

    CircleCollider2D targetAreaCol;

    // Properties from PlayerState for convenience
    private float reloadSpeed => playerState.roundStats.reloadSpeed;
    private int magazineSize => playerState.roundStats.magazineSize;
    private float fireDelay => playerState.roundStats.fireDelay;
    private float spreadAngle => playerState.roundStats.bulletSpread;
    private float bulletSpeed => playerState.roundStats.bulletSpeed;
    private int bulletsPerBurst => playerState.roundStats.bulletsPerBurst;
    private float burstDelay => playerState.roundStats.burstDelay;
    private float targetingRange => playerState.roundStats.targetingRange;


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
                    layerMask = LayerMask.GetMask("Players"),
                    useTriggers = false
                },
                LayerMask.GetMask("Solid", "Players")
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
        // Generate bullet ID
        int bulletId = playerID * 1000 + nextBulletId++;

        // Calculate velocity
        Vector2 velocity = bullet2DVelocity(bulletSpeed, transform.eulerAngles.z, spreadAngle);

        // Create bullet state using PlayerRoundStats method
        BulletState newBulletState = playerState.roundStats.GenerateBulletState(
            bulletId,
            playerID,
            velocity
        );

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
        EntityRegistry.bullets.Add(bulletId, bulletGO);

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
        yield return new WaitForSeconds(reloadSpeed);
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
            // Skip if the hit is this gun's own collider or a child of this transform
            if (hit.transform == transform || hit.transform.IsChildOf(transform))
                continue;

            if (hit.transform.TryGetComponent<PlayerHandler>(out PlayerHandler playerHandler))
            {
                if (playerHandler.playerID == playerID)
                {
                    // Skip if the hit is the player who owns this gun
                    continue;
                }
            }

            Vector2 dir = (hit.bounds.center - (Vector3)origin);
            float dist = dir.magnitude;

            // Start raycast slightly offset to avoid hitting self
            RaycastHit2D los = Physics2D.Raycast(
                origin + dir.normalized * 0.1f,
                dir.normalized,
                dist - 0.1f,
                obstacleMask
            );

            // If the first thing we hit is the target, it's visible
            // If nothing was hit (los.collider == null), the path is clear
            if (los.collider == null || los.collider == hit)
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
