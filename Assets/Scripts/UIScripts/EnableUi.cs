using UnityEngine;

public class EnableUi : MonoBehaviour
{
    public GameObject cardSelectMenu;
    public TowerHealth[] towerHealths;          // up to 4 towers
    public UpgradeCardSpawner UpgradeCardSpawner;

    void Update()
    {
        if (AllTowersDead())
        {
            UpgradeCardSpawner.GiveRandomUpgrade();
            cardSelectMenu.SetActive(true);

            // reset so it doesn't trigger every frame
            foreach (TowerHealth tower in towerHealths)
            {
                tower.isDead = false;
            }
        }
    }

    bool AllTowersDead()
    {
        if (towerHealths == null || towerHealths.Length == 0)
            return false;

        foreach (TowerHealth tower in towerHealths)
        {
            if (!tower.isDead)
                return false;
        }

        return true;
    }
}
