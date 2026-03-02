using JetBrains.Annotations;
using System;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{

    [NonSerialized] public GameStateHandler gameStateHandler; // assigned on instantiation in GameStateHandler
    [NonSerialized] public int playerID; // assigned on instantiation in GameStateHandler
    [NonSerialized] public PlayerState playerState;
    [NonSerialized] public int animationIndex;

    bool wasDead = false;
    public bool isDead = false;

    float currentHealth;


    // Start is called before the first frame update
    void Start()
    {
        playerState = gameStateHandler.gameState.currentRound.players[playerID];
        gameObject.name = "Player_" + playerID;

        // Calling the setup for the movement script
        GetComponent<PlayerMovement>().SetupVars();
        // Setting animation to show
        GetComponent<Animator>().SetLayerWeight(animationIndex, 1);

        currentHealth = playerState.roundStats.maxHealth;


    }

    void InflictDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Kill();
        }
    }

    void Kill()
    {
        isDead = true;
        gameObject.SetActive(false);
        gameStateHandler.PlayerDeath(playerID);
    }

    // Update is called once per frame
    void Update()
    {
        // For debuging
        if (isDead && !wasDead)
        { 
            wasDead = true;
            Kill();
        }
    }
}
