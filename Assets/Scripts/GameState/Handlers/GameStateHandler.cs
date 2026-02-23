using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class GameStateHandler : MonoBehaviour
{ 
    [SerializeField] public int roundsToWin = 3;
    [SerializeField] public GameObject playerPrefab;
    [SerializeField] public bool debugStartRound = false;

    public GameState gameState;

    Gamepad[] usedGamePads = new Gamepad[4];
    bool lastDebugStartRound = false;
    int playerCount = 0;


    // Start is called before the first frame update
    void Start()
    {
        gameState = new GameState
        {
            status = GameStatus.Lobby,
            playerProfiles = new PlayerGameProfile[4],
            roundsToWin = 3
        };

        // Adding a player for testing
        AddNewPlayerProfile();
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
    void StartRound()
    {
        // Setting up the new round
        gameState.status = GameStatus.InRound;
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
            // Setting up the player handler
            currentPlayer.GetComponent<PlayerHandler>().gameStateHandler = this;
            currentPlayer.GetComponent<PlayerHandler>().playerID = i;
        }
    }


    // Update is called once per frame
    void Update()
    {
        // Round start debug triger
        if (debugStartRound != lastDebugStartRound)
        {
            StartRound();
        }
        lastDebugStartRound = debugStartRound;

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
                }
            }
        }
    }
}
