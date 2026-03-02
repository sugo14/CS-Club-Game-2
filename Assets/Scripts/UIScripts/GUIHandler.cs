using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.UI;

public class GUIHandler : MonoBehaviour
{
    public GameObject lobbyMenu, startButton, playerCountTextBox;
    public GameObject cardSelectMenu;
    public GameObject cards; // parent object of the 4 cards

    UpgradeCardSpawner cardSpawner;
    GameObject[] cardObjects = new GameObject[4];
    GameStateHandler GSH;

    void Start()
    {
        // Getting the card spawner script
        cardSpawner = cards.GetComponent<UpgradeCardSpawner>();

        // Getting the card objects
        for (int i = 0; i < 4; i++)
        { 
            cardObjects[i] = cards.transform.GetChild(i).gameObject;
        }
    }

    public void LinkGSH(GameStateHandler gameStateHandler)
    {
        GSH = gameStateHandler;
    }

    public void CardSelectMenu(bool show = true)
    {
        cardSelectMenu.SetActive(show);
        foreach (GameObject card in cardObjects) { card.SetActive(show); }
        cardSpawner.GiveRandomUpgrade();
        if (show)
        {
            MultiplayerEventSystem.current.SetSelectedGameObject(cardObjects[0]);
        }
    }

    public void LobbyMenu(bool show = true)
    {
        lobbyMenu.SetActive(show);
        if (show)
        {
            UpdatePlayerCount();
            MultiplayerEventSystem.current.SetSelectedGameObject(startButton);
        }
    }

    public void UpdatePlayerCount()
    {
        playerCountTextBox.GetComponent<TextMeshProUGUI>().text = "Press Menu to Join\nPlayer Count: " + GSH.playerCount;
    }

}
