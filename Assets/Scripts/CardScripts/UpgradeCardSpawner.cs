using System.Collections.Generic;
using UnityEngine;

public class UpgradeCardSpawner : MonoBehaviour
{
    public GameObject cards;      // parent object of the 4 cards
    public List<Upgrade> pickedUpgrades = new List<Upgrade>();

    // A array for the names of the picked upgrades
    [System.NonSerialized]
    public string[] upgrades = new string[4];

    CardUI[] cardUIs = new CardUI[4];

    void Start()
    {
        // Getting the CardUI components of the 4 cards
        for (int i = 0; i < cards.transform.childCount; i++)
        {
            cardUIs[i] = cards.transform.GetChild(i).GetComponent<CardUI>();
        }
    }

    public void GiveRandomUpgrade()
    {
        // If the cardUIs array was not populated in Start() from loading order, populate it now
        if (!cardUIs[0])
        {
            for (int i = 0; i < cards.transform.childCount; i++)
            {
                cardUIs[i] = cards.transform.GetChild(i).GetComponent<CardUI>();
            }
        }

        foreach (var card in cardUIs)
        {
            card.ClearCoverImage();

            int randomIndex = Random.Range(0, Upgrades.db.Length);
            Upgrade selectedUpgrade = Upgrades.db[randomIndex];

            card.SetSpawner(this);          //important
            card.SetUpgrade(selectedUpgrade);
        }
    }

    public void RegisterChoice(Upgrade upgrade)
    {
        pickedUpgrades.Add(upgrade);

        int count = pickedUpgrades.Count;

        upgrades[count - 1] = upgrade.name;

        Debug.Log("Picked #" + count + " " + upgrade.name);
    }

}