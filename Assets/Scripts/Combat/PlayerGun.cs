using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerGun : MonoBehaviour
{
    //TODO: Set this through code from the registery
    [SerializeField]
    int playerID = 1;
    [SerializeField]
    float spreadAngle = 0.08f; //degrees

    [SerializeField]
    private GameObject bulletPrefab;


    int nextBulletId = 000;


    void Start()
    {

    }

    void Update()
    {

    }

    public void Fire()
    {
        CreateBullet();
    }
    //Not done yet
    public void Reload()
    {
        Debug.Log("Reload");
    }

    // Create and initialize a new bullet
    GameObject CreateBullet()
    {

        Debug.Log("CreateBullet");
        // Create a new bullet state
        BulletState newBulletState = new BulletState
        {
            id = playerID * 1000 + nextBulletId++,
            ownerId = playerID,
            //set this through code leter
            damage = 10f,
            knockback = 5f,
            lifestealPercent = 0f,


            velocity = bullet2DVelocity(20f, transform.eulerAngles.z, spreadAngle)


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


}
