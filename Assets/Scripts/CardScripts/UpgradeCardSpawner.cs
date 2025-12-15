using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeCardSpawner : MonoBehaviour
{
    public CardUI[] cardUIs;
    // Start is called before the first frame update
    void Start()
    {
        GiveRandomUpgrade();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GiveRandomUpgrade()
    {
        foreach (CardUI card in cardUIs)
        {
            
        // Pick a random uprade from the database
        int randomIndex = Random.Range(0, Upgrades.db.Length);
        Upgrade randomUpgrade = Upgrades.db[randomIndex];

        // Assign it to the card UI
        card.SetUpgrade(randomUpgrade);
        }
    }
}
