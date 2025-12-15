using UnityEngine;

public class CardTest : MonoBehaviour
{
    public CardUI cardUI;       
    public CardData cardData;  

    private void Awake()
    {
        if (cardUI != null && cardData != null)
        {
            cardUI.cardData = cardData;  // assign the ScriptableObject
            cardUI.UpdateCardUI();        // initialize UI
        }
    }

    private void Update()
    {
        // Forces dynamic update each frame (optional if using version polling)
        if (cardUI != null)
            cardUI.UpdateCardUI();
    }
}
