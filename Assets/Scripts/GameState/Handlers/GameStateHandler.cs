using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class GameStateHandler : MonoBehaviour
{
    [Header("Player Spawn Settings")]
    public GameObject playerPrefab;
    public Vector2[] spawnPoints = new Vector2[4];
    public int[] playerAnimations = new int[4];
    [Header("Gun Spawn Settings")]
    public GameObject gunPrefab;

    [Header("Tower Settings")]
    public GameObject towerPrefab;
    public Vector2[] towerSpawnPoints = new Vector2[4];
    [Header("Game Settings")]
    public int roundsToWin = 3;
    [Header("Debug")]
    public bool SpawnKeyboardPlayer = false;

    public GameState gameState;

    [NonSerialized]
    public int playerCount = 0;
    GUIHandler UIHandler;
    Gamepad[] usedGamePads = new Gamepad[4];
    bool[] playersAlive = new bool[4];


    void Start()
    {
        UIHandler = GetComponent<GUIHandler>();
        UIHandler.LinkGSH(this);

        gameState = new GameState
        {
            playerProfiles = new PlayerGameProfile[4],
            wins = new int[4],
            status = GameStatus.Lobby,

            roundsToWin = 3
        };

        // Adding a player controlled by the keyboard for testing
        if (SpawnKeyboardPlayer)
        {
            AddNewPlayerProfile();
        }

        // Showing the lobby menu
        UIHandler.LobbyMenu();
    }


    void AddNewPlayerProfile(int gamePadID = -1)
    {
        PlayerGameProfile newPlayer = new PlayerGameProfile
        {
            id = gameState.playerProfiles.Length,
            playerName = "Player" + gameState.playerProfiles.Length,
            upgrades = new List<Upgrades.ID>(),
            gamePadId = gamePadID
        };
        gameState.playerProfiles[playerCount] = newPlayer;
        playerCount++;
    }


    // Returns the gamepad with the given id, or null if it doesn't exist.
    Gamepad GetGamepadByID(int id)
    {
        foreach (var gamePad in Gamepad.all)
        {
            if (gamePad.deviceId == id)
            {
                return gamePad;
            }
        }
        return null;
    }

    // Starts a new round
    public void StartRound()
    {
        // Preventing starting a new round if already in one or if there are no players
        if (gameState.status == GameStatus.InRound || playerCount < 1)
        {
            return;
        }

        // Setting up the new round
        gameState.status = GameStatus.InRound;
        UIHandler.LobbyMenu(false); // Hiding the lobby menu

        gameState.currentRound = new()
        {
            level = LevelID.Level1,
            players = new PlayerState[gameState.playerProfiles.Length]
        };

        playersAlive = new bool[] { true, true, true, true };

        // Initializing players
        GameObject currentPlayer;
        GameObject currentTower;
        GameObject currentGun;

        for (int i = 0; i < playerCount; i++)
        {
            // Spawning tower
            currentTower = Instantiate(towerPrefab, towerSpawnPoints[i], Quaternion.identity);
            currentTower.name = "Tower_" + i;

            gameState.currentRound.players[i] = gameState.playerProfiles[i].GenerateInitialPlayerState();

            // Register tower in EntityRegistry
            EntityRegistry.towers.Add(i, currentTower);
            int currentGamepadID = gameState.playerProfiles[i].gamePadId;

            if (currentGamepadID == -1) // If the player doesn't have a gamepad, pair it with the keyboard
            {
                currentPlayer = PlayerInput.Instantiate(playerPrefab,
                    controlScheme: "Keyboard",
                    pairWithDevice: Keyboard.current).gameObject;
            }
            else // If the player has a gamepad, pair it with the correct one
            {
                currentPlayer = PlayerInput.Instantiate(playerPrefab,
                    controlScheme: "Gamepad",
                    pairWithDevice: usedGamePads[i]).gameObject;
            }
            // spawning gun
            currentGun = Instantiate(gunPrefab, new Vector2(0, 0), Quaternion.identity);
            currentGun.transform.SetParent(currentPlayer.transform, false);
            currentGun.transform.position += new Vector3(0, 1, 0); 

            // Setting up the gun
            Gun gunComponent = currentGun.GetComponent<Gun>();
            gunComponent.playerID = i;
            gunComponent.playerState = gameState.currentRound.players[i];
            gunComponent.player = currentPlayer;

            // Setting up the player
            currentPlayer.GetComponent<PlayerHandler>().gameStateHandler = this;
            currentPlayer.GetComponent<PlayerHandler>().playerID = i;
            currentPlayer.transform.position = spawnPoints[i];
            currentPlayer.GetComponent<PlayerHandler>().animationIndex = playerAnimations[i];

            // Register player in EntityRegistry
            EntityRegistry.players.Add(i, currentPlayer);

            // Pairing the player with their tower
            TowerHealth towerHealth = currentTower.GetComponent<TowerHealth>();
            towerHealth.player = currentPlayer;
            towerHealth.playerID = i;
            towerHealth.playerState = gameState.currentRound.players[i];


            // Try to find a Gun component in the tower's children
            Gun towerGun = currentTower.GetComponentInChildren<Gun>();
            if (towerGun != null)
            {
                towerGun.playerID = i;
                towerGun.playerState = gameState.currentRound.players[i];
                towerGun.player = currentPlayer;
            }
            else
            {
                Debug.LogWarning($"Tower {i} is supposed to have a gun but no Gun component found in children.");
            }


        }
    }

    void EndRound(int winnerID)
    {
        gameState.status = GameStatus.BetweenRounds;
        gameState.wins[winnerID]++;
        Debug.Log("Player " + winnerID + " wins the round!");



        UIHandler.CardSelectMenu();
    }

    public void PlayerDeath(int playerID)
    {
        playersAlive[playerID] = false;

        int deathCount = 0;
        int winnerID = -1;

        for (int i = 0; i < playerCount; i++)
        {
            if (playersAlive[i])
            {
                winnerID = i;
            }
            else
            {
                deathCount++;
            }
        }
        Debug.Log("Player " + playerID + " died. Death count: " + deathCount);
        if (deathCount == playerCount - 1)
        {
            EndRound(winnerID);
        }
    }


    void Update()
    {
        if (gameState.status == GameStatus.Lobby)
        {
            // Checking for new players
            foreach (var gamePad in Gamepad.all)
            {
                if (gamePad.startButton.wasPressedThisFrame)
                {
                    foreach (var usedPad in usedGamePads)
                    {
                        if (gamePad == usedPad)
                        {
                            return;
                        }
                    }
                    usedGamePads[playerCount] = gamePad;
                    AddNewPlayerProfile(gamePad.deviceId);
                    UIHandler.UpdatePlayerCount(); // Updating the lobby menu to show the new player
                }
            }
        }
    }
}
