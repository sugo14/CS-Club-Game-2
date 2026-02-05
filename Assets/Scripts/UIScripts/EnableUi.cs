using UnityEngine;

public class EnableUi : MonoBehaviour
{
    public GameObject UI;
    public towerHealth[] towerHealths;          // up to 4 towers
    public UpgradeCardSpawner UpgradeCardSpawner;

    void Start()
    {
    }

    void Update()
    {
        if (AllTowersDead())
        {
            UpgradeCardSpawner.GiveRandomUpgrade();
            UI.SetActive(true);

            // reset so it doesn't trigger every frame
            foreach (towerHealth tower in towerHealths)
            {
                tower.IS_DEAD = false;
            }
        }
    }

    bool AllTowersDead()
    {
        if (towerHealths == null || towerHealths.Length == 0)
            return false;

        foreach (towerHealth tower in towerHealths)
        {
            if (!tower.IS_DEAD)
                return false;
        }

        return true;
    }
}
