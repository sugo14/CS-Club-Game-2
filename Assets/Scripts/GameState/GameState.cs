using System.Collections.Generic;

// Status of the overall game.
public enum GameStatus
{
    Lobby,
    InRound,
    BetweenRounds,
    GameOver
}

// Info kept throughout the game, across rounds.
public struct GameState
{
    // runtime info
    public PlayerGameProfile[] playerProfiles;
    public int[] wins;
    public GameStatus status;
    public RoundState currentRound;

    // game rules
    public readonly int roundsToWin;
}

// Info kept for each player throughout the game, across rounds.
public struct PlayerGameProfile
{
    public int id;
    public string playerName;
    public List<Upgrades.ID> upgrades;

    public readonly PlayerState GenerateInitialPlayerState()
    {
        // accumulate and calculate stats
        float damageMult = 1, knockbackMult = 1, lifestealAdd = 0, bulletSpeedMult = 1;
        float bulletWavesMult = 1, bulletsPerWaveMult = 1, bulletSpreadMult = 1;
        float moveSpeedMult = 1, maxHealthMult = 1;
        float maxTowerHealthMult = 1;
        bool towerHasGun = false, seekingBullets = false;

        foreach (int uid in upgrades)
        {
            Upgrade u = Upgrades.db[uid];

            damageMult *= u.damageMult;
            knockbackMult *= u.knockbackMult;
            lifestealAdd += u.lifestealAdd;
            bulletSpeedMult *= u.bulletSpeedMult;

            bulletWavesMult *= u.bulletWavesMult;
            bulletsPerWaveMult *= u.bulletsPerWaveMult;
            bulletSpreadMult *= u.bulletSpreadMult;

            moveSpeedMult *= u.moveSpeedMult;
            maxHealthMult *= u.maxHealthMult;

            maxTowerHealthMult *= u.maxTowerHealthMult;

            if (u.towerHasGun) { towerHasGun = true; }
            if (u.seekingBullets) { seekingBullets = true; }
        }

        PlayerRoundStats stats = new PlayerRoundStats(
            damageMult, knockbackMult, lifestealAdd, bulletSpeedMult,
            bulletWavesMult, bulletsPerWaveMult, bulletSpreadMult,
            moveSpeedMult, maxHealthMult,
            maxTowerHealthMult,
            towerHasGun, seekingBullets
        );
        
        // compile into initial PlayerState
        PlayerState ps = new PlayerState(
            id,
            stats
        );

        return ps;
    }
}
