using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{

    [NonSerialized] public GameStateHandler gameStateHandler; // assigned on instantiation in GameStateHandler
    [NonSerialized] public int playerID; // assigned on instantiation in GameStateHandler
    [NonSerialized] public PlayerState playerState;

    // Start is called before the first frame update
    void Start()
    {
        playerState = gameStateHandler.gameState.currentRound.players[playerID];
        gameObject.name = "Player_" + playerID;

        // Calling the setup for the movement script
        GetComponent<PlayerMovement>().SetupVars();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
