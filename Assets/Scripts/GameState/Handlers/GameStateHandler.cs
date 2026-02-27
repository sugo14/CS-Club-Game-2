using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class GameStateHandler : MonoBehaviour
{
    [Header("Player Spawn Settings")]
    public GameObject playerPrefab;
    public Vector2[] spawnPoints = new Vector2[4];
    public int[] playerAnimations = new int[4];
    [Header("Game Settings")]
    public int roundsToWin = 3;
    [Header("Debug")]
    public bool SpawnKeyboardPlayer = false;

    public GameState gameState;

    [NonSerialized]
    public int playerCount = 0;
    GUIHandler UIHandler;
    Gamepad[] usedGamePads = new Gamepad[4];


    void Start()
    {
        UIHandler = GetComponent<GUIHandler>();
        UIHandler.LinkGSH(this);

        gameState = new GameState
        {
            status = GameStatus.Lobby,
            playerProfiles = new PlayerGameProfile[4],
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


        // Initializing players
        GameObject currentPlayer;
        for (int i = 0; i < playerCount; i++)
        {
           
            gameState.currentRound.players[i] = gameState.playerProfiles[i].GenerateInitialPlayerState();
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
            // Setting up the player
            currentPlayer.GetComponent<PlayerHandler>().gameStateHandler = this;
            currentPlayer.GetComponent<PlayerHandler>().playerID = i;
            currentPlayer.transform.position = spawnPoints[i];
            currentPlayer.GetComponent<PlayerHandler>().animationIndex = playerAnimations[i];
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
