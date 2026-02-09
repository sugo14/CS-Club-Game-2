// A single upgrade's stats and flags.
public readonly struct Upgrade
{
    public readonly int id; // map to UpgradeID
    public readonly string name, description; // display info
    public readonly float damageMult, knockbackMult, lifestealAdd, bulletSpeedMult; // bullet stats
    public readonly float bulletWavesMult, bulletsPerWaveMult, bulletSpreadMult; // bullet firing stats
    public readonly float moveSpeedMult, jumpForceMult, maxHealthMult; // player stats
    public readonly float maxTowerHealthMult; // tower stats
    public readonly bool towerHasGun, seekingBullets; // specific upgrade flags

    public Upgrade(
        int id, string name, string description,
        float damageMult = 1f, float knockbackMult = 1f, float lifestealAdd = 0f, float bulletSpeedMult = 1f,
        float bulletWavesMult = 1f, float bulletsPerWaveMult = 1f, float bulletSpreadMult = 1f,
        float moveSpeedMult = 1f, float jumpForceMult = 1f, float maxHealthMult = 1f,
        float maxTowerHealthMult = 1f,
        bool towerHasGun = false, bool seekingBullets = false
    ) {
        this.id = id;
        this.name = name;
        this.description = description;

        this.damageMult = damageMult;
        this.knockbackMult = knockbackMult;
        this.lifestealAdd = lifestealAdd;
        this.bulletSpeedMult = bulletSpeedMult;

        this.bulletWavesMult = bulletWavesMult;
        this.bulletsPerWaveMult = bulletsPerWaveMult;
        this.bulletSpreadMult = bulletSpreadMult;

        this.moveSpeedMult = moveSpeedMult;
        this.jumpForceMult = jumpForceMult;
        this.maxHealthMult = maxHealthMult;

        this.maxTowerHealthMult = maxTowerHealthMult;

        this.towerHasGun = towerHasGun;
        this.seekingBullets = seekingBullets;
    }
}

// Database of all available upgrades.
public static class Upgrades
{
    public enum ID
    {
        DamageBoost, // +10% damage
        DamageBoost2, // +15% damage
        Knockback, // +50% knockback
        DoubleWaves, // -25% damage, 2x waves
        Shotgun, // -60% damage, 5x bullets, +20% spread
        Lifesteal, // +20% lifesteal
        SpeedBoost, // +20% move speed
        Health, // +10% max health
        Health2, // +15% max health
        Tank, // +30% max health, -20% move speed
        TowerHealth , // +10% tower health
        TowerGun  // toggle tower gun
    }

    public static readonly Upgrade[] db =
    {
        new Upgrade(0, "Damage Boost", "+10% damage", damageMult: 1.1f),
        new Upgrade(1, "Damage Boost 2", "+15% damage", damageMult: 1.15f),
        new Upgrade(2, "Knockback", "+50% knockback", knockbackMult: 1.5f),
        new Upgrade(3, "Double Waves", "-25% damage, 2x bullet waves", damageMult: 0.75f, bulletWavesMult: 2f),
        new Upgrade(4, "Shotgun", "-60% damage, 5x bullets, +20% spread", damageMult: 0.4f, bulletsPerWaveMult: 5f, bulletSpreadMult: 1.2f),
        new Upgrade(5, "Lifesteal", "+20% lifesteal", lifestealAdd: 0.2f),
        new Upgrade(6, "Speed Boost", "+20% move speed", moveSpeedMult: 1.2f),
        new Upgrade(7, "Health", "+10% max health", maxHealthMult: 1.1f),
        new Upgrade(8, "Health 2", "+15% max health", maxHealthMult: 1.15f),
        new Upgrade(9, "Tank", "+30% max health, -20% move speed", maxHealthMult: 1.3f, moveSpeedMult: 0.8f),
        new Upgrade(10, "Tower Health", "+10% tower health", maxTowerHealthMult: 1.1f),
        new Upgrade(11, "Tower Gun", "Add gun to tower", towerHasGun: true)
    };
}
