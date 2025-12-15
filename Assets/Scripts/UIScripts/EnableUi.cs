using UnityEngine;

public class EnableUi : MonoBehaviour
{
    public GameObject UI;
    public towerHealth towerHealth;
    public UpgradeCardSpawner UpgradeCardSpawner;

    void Start()
    {

    }

    void Update()
    {
        if (towerHealth.IS_DEAD)
        {
            UpgradeCardSpawner.GiveRandomUpgrade();
            UI.SetActive(true);
            towerHealth.IS_DEAD = false;
        }
    }
}
