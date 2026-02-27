using UnityEngine;

// Base stats for all players before upgrades.
public static class PlayerBaseStats
{
    public const float baseDamage = 10f;
    public const float baseKnockback = 5f;
    public const float baseLifesteal = 0f;
    public const float baseBulletSpeed = 5f;


    public const int baseBulletsPerBurst = 1;
    public const float baseBurstDelay = 50f; //milliseconds
    public const float baseBulletSpread = 0.08f; // deg
    public const float baseReloadSpeed = 2f;
    public const float baseFireDelay = 0.1f;
    public const int baseMagazineSize = 3;

    public const float baseMoveSpeed = 10f;
    public const float baseJumpForce = 12f;
    public const float baseMaxHealth = 100;

    public const float baseTargetingRange = 5f;

    public const float baseMaxTowerHealth = 200;
}

// Info kept for each player within a round.
public struct PlayerState
{
    public int id; // map to gameobject / gameprofile

    public PlayerRoundStats roundStats;
    public PlayerRuntimeStats runtimeStats;

    public PlayerState(int id, PlayerRoundStats roundStats)
    {
        this.id = id;
        this.roundStats = roundStats;
        this.runtimeStats = roundStats.GenerateInitialRuntimeStats();
    }
}

// A player's stats and flags for a round, generated from upgrades.
public readonly struct PlayerRoundStats
{
    // bullet stats
    public readonly float damage, knockback, lifesteal, bulletSpeed;

    // bullet firing stats
    public readonly int  bulletsPerBurst, magazineSize;
    public readonly float bulletSpread, burstDelay, reloadSpeed, fireDelay;
    
    // player stats
    public readonly float moveSpeed, jumpForce, maxHealth;

    // tower stats
    public readonly float maxTowerHealth, targetingRange;

    // specific upgrade flags
    public readonly bool towerHasGun, seekingBullets;

    // construct relative to base stats
    // Backward-compatible constructor matching the old parameter list
    public PlayerRoundStats(
        float damageMult, float knockbackMult, float lifestealAdd, float bulletSpeedMult,
        float bulletWavesMult, float bulletsPerWaveMult, float bulletSpreadMult, float magazineSizeMult, float burstDelayMult, float reloadspeedMult,
        float moveSpeedMult, float jumpForceMult, float maxHealthMult,
        float maxTowerHealthMult,
        bool towerHasGun, bool seekingBullets
    ) : this(
        damageMult, knockbackMult, lifestealAdd, bulletSpeedMult,
        /* bulletsPerBurstMult */ bulletWavesMult * bulletsPerWaveMult,
        bulletSpreadMult, magazineSizeMult, burstDelayMult, reloadspeedMult, 
        /* fireDelayMult (new) */ 1f,
        moveSpeedMult, jumpForceMult, maxHealthMult,
        maxTowerHealthMult, 
        /* targetingRangeMult (new) */ 1f,
        towerHasGun, seekingBullets
    )
    {
    }

    // New constructor including fireDelayMult and targetingRangeMult
    public PlayerRoundStats(
        float damageMult, float knockbackMult, float lifestealAdd, float bulletSpeedMult,
        float bulletsPerBurstMult, float bulletSpreadMult, float magazineSizeMult, float burstDelayMult, float reloadspeedMult, float fireDelayMult,
        float moveSpeedMult, float jumpForceMult, float maxHealthMult,
        float maxTowerHealthMult, float targetingRangeMult,
        bool towerHasGun, bool seekingBullets
    ) {
        this.damage = PlayerBaseStats.baseDamage * damageMult;
        this.knockback = PlayerBaseStats.baseKnockback * knockbackMult;
        this.lifesteal = PlayerBaseStats.baseLifesteal + lifestealAdd;
        this.bulletSpeed = PlayerBaseStats.baseBulletSpeed * bulletSpeedMult;
        this.fireDelay = PlayerBaseStats.baseFireDelay * fireDelayMult;

        this.bulletsPerBurst = Mathf.RoundToInt(PlayerBaseStats.baseBulletsPerBurst * bulletsPerBurstMult);
        this.bulletSpread = PlayerBaseStats.baseBulletSpread * bulletSpreadMult;
        this.magazineSize = Mathf.RoundToInt(PlayerBaseStats.baseMagazineSize * magazineSizeMult);
        this.burstDelay = PlayerBaseStats.baseBurstDelay * burstDelayMult;
        this.reloadSpeed = PlayerBaseStats.baseReloadSpeed * reloadspeedMult;

        this.moveSpeed = PlayerBaseStats.baseMoveSpeed * moveSpeedMult;
        this.jumpForce = PlayerBaseStats.baseJumpForce * jumpForceMult;
        this.maxHealth = PlayerBaseStats.baseMaxHealth * maxHealthMult;
        
        this.maxTowerHealth = PlayerBaseStats.baseMaxTowerHealth * maxTowerHealthMult;
        this.targetingRange = PlayerBaseStats.baseTargetingRange * targetingRangeMult;
        this.towerHasGun = towerHasGun;
        this.seekingBullets = seekingBullets;
    }

    public PlayerRuntimeStats GenerateInitialRuntimeStats()
    {
        return new PlayerRuntimeStats(
            currHealth: (int)maxHealth,
            currTowerHealth: (int)maxTowerHealth
        );
    }

    public BulletState GenerateBulletState(int bulletId, int ownerId, Vector3 velocity)
    {
        return new BulletState
        {
            id = bulletId,
            ownerId = ownerId,
            damage = damage,
            knockback = knockback,
            lifestealPercent = lifesteal,
            velocity = velocity
        };
    }
}

// A player's stats that can be updated during a round.
public struct PlayerRuntimeStats
{
    public int currHealth, currTowerHealth;

    public PlayerRuntimeStats(int currHealth, int currTowerHealth)
    {
        this.currHealth = currHealth;
        this.currTowerHealth = currTowerHealth;
    }

    // TODO: add position and velocity when networking
}

// TODO: add serializable vector3 when networking
