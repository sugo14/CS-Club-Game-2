using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GameStateHandler : MonoBehaviour
{ 
    [SerializeField] public int roundsToWin = 3;
    [SerializeField] public GameObject playerPrefab;

    public GameState gameState;

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

        // Starting round for testing
        StartRound();
    }


    void AddNewPlayerProfile()
    {
        PlayerGameProfile newPlayer = new PlayerGameProfile
        {
            id = gameState.playerProfiles.Length,
            playerName = "Player" + gameState.playerProfiles.Length,
            upgrades = new List<Upgrades.ID>()
        };
        gameState.playerProfiles[playerCount] = newPlayer;
        playerCount++;
    }

    // Starts a new round
    void StartRound()
    {
        gameState.status = GameStatus.InRound;
        gameState.currentRound = new()
        {
            level = LevelID.Level1,
            players = new PlayerState[gameState.playerProfiles.Length]
        };

        // Initializing players
        for (int i = 0; i < playerCount; i++)
        {
            gameState.currentRound.players[i] = gameState.playerProfiles[i].GenerateInitialPlayerState();
            GameObject currentPlayer = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            currentPlayer.GetComponent<PlayerHandler>().gameStateHandler = this;
            currentPlayer.GetComponent<PlayerHandler>().playerID = i;
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
