using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.UI;

public class GUIHandler : MonoBehaviour
{
    public GameObject lobbyMenu, startButton, playerCountTextBox;

    GameStateHandler GSH;

    public void LinkGSH(GameStateHandler gameStateHandler)
    {
        GSH = gameStateHandler;
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
