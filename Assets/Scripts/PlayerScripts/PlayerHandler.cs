using JetBrains.Annotations;
using System;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public GameObject healthBarPrefab;
    public float healthBarHeight = 1;

    [NonSerialized] public GameStateHandler gameStateHandler; // assigned on instantiation in GameStateHandler
    [NonSerialized] public int playerID; // assigned on instantiation in GameStateHandler
    [NonSerialized] public PlayerState playerState;
    [NonSerialized] public int animationIndex;
    [NonSerialized] public GameObject playerGun;

    GameObject healthBar;

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

        // Add health bar
        healthBar = Instantiate(healthBarPrefab, transform.position + new Vector3(0, healthBarHeight), Quaternion.identity);
        healthBar.transform.SetParent(transform, true);

    }

    void ShowPlayer(bool show = true)
    {
        GetComponent<SpriteRenderer>().enabled = show;
        GetComponent<CircleCollider2D>().enabled = show;
        GetComponent<PlayerMovement>().enabled = show;
        healthBar.SetActive(show);
        playerGun.SetActive(show);

    }

    void UpdateHealthBar()
    {
        healthBar.GetComponent<HealhBar>().setFill(currentHealth / playerState.roundStats.maxHealth);

    }

    public void InflictDamage(float damage)
    {
        currentHealth -= damage;
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Respawn();
        }
    }

    void Respawn()
    {
        tryRespawn = true;
        ShowPlayer(false);
    }

    public void Kill()
    {
        ShowPlayer(false);
        currentHealth = 0;
        UpdateHealthBar();
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
                UpdateHealthBar();
                transform.position = gameStateHandler.spawnPoints[playerID];
                isDead = false;
                ShowPlayer();
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
