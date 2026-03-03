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

    bool tryRespawn = false;
    float respawnTimer = 0f;
    public float respawnTime = 3f;

    

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

    public void InflictDamage(float damage)
    {
        currentHealth -= damage;
        print(currentHealth);
        if (currentHealth <= 0)
        {
            Respawn();
        }
    }

    void Respawn()
    {
        tryRespawn = true;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void Kill()
    {
        isDead = true;
        gameObject.SetActive(false);
        gameStateHandler.PlayerDeath(playerID);
    }

    // Update is called once per frame
    void Update()
    {
        // Repawn player after respawn time
        if (tryRespawn)
        {
            respawnTimer += Time.deltaTime;

            if (respawnTimer >= respawnTime)
            {
                tryRespawn = false;
                respawnTimer = 0f;
                currentHealth = playerState.roundStats.maxHealth;
                transform.position = gameStateHandler.spawnPoints[playerID];
                isDead = false;
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                
            }

        }

        // For debuging
        if (isDead && !wasDead)
        { 
            Respawn();
            //wasDead = true;
            //Kill();
        }
    }
}
