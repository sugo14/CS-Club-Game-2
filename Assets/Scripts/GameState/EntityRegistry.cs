using UnityEngine;
using System.Collections.Generic;

// Maps IDs to GameObjects, effectively connecting RoundState to Unity,
public static class EntityRegistry
{
    // player id -> gameobject
    public static Dictionary<int, GameObject> players = new Dictionary<int, GameObject>();
    public static Dictionary<int, GameObject> towers = new Dictionary<int, GameObject>();

    // bullet id -> gameobject
    public static Dictionary<int, GameObject> bullets = new Dictionary<int, GameObject>();

    public static void Clear()
    {
        players.Clear();
        towers.Clear();
        bullets.Clear();
    }
}
