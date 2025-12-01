using UnityEngine;
using System.Collections.Generic;

// Info kept for the current round.
public struct RoundState
{
    public PlayerState[] players;
    public List<BulletState> bullets;
    public LevelID level;
}

// Enum of all levels.
// ! idk if this is necessary or good
public enum LevelID
{
    Level1,
    Level2
}
