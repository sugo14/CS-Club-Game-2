using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeCardSpawner : MonoBehaviour
{
    public CardUI[] cardUIs;

    //Assign random upgrades to cards and update visuals
    public void GiveRandomUpgrade()
    {
        foreach (var card in cardUIs)
        {
            // Clear old cover image first
            card.ClearCoverImage();

            // Pick a random upgrade from database
            int randomIndex = Random.Range(0, Upgrades.db.Length);
            Upgrade selectedUpgrade = Upgrades.db[randomIndex];

            // Assign upgrade (updates texts, cover, and background)
            card.SetUpgrade(selectedUpgrade);
        }
    }

    private void Start()
    {
        GiveRandomUpgrade();
    }
}
