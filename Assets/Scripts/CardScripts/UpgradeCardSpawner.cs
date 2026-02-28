using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeCardSpawner : MonoBehaviour
{
    public CardUI[] cardUIs;

    public List<Upgrade> pickedUpgrades = new List<Upgrade>();

    public string UPGRADE1;
    public string UPGRADE2;
    public string UPGRADE3;
    public string UPGRADE4;

    public void GiveRandomUpgrade()
    {
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

        if (count == 1) UPGRADE1 = upgrade.name;
        if (count == 2) UPGRADE2 = upgrade.name;
        if (count == 3) UPGRADE3 = upgrade.name;
        if (count == 4) UPGRADE4 = upgrade.name;

        Debug.Log("Picked #" + count + " " + upgrade.name);
    }

    private void Start()
    {
        GiveRandomUpgrade();
    }
}