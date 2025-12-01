using UnityEngine;

// Info kept for each bullet fired.
public struct BulletState
{
    public int id; // map to gameobject
    public int ownerId; // map to owner state and gameobject

    public float damage, knockback;
    public float lifestealPercent;
    public Vector3 velocity;
    
    // TODO: add position when networking
}
