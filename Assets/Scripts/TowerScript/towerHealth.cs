using UnityEngine;

public class TowerHealth : MonoBehaviour
{
    public int towerHealth;
    public bool isDead;
    public GameObject player;

    [System.NonSerialized] public int playerID;
    [System.NonSerialized] public PlayerState playerState;

    void Start()
    {
        isDead = false;
        gameObject.SetActive(true);

        // Initialize tower health from player state
        if (playerState.id != 0 || playerID == 0)
        {
            towerHealth = (int)playerState.roundStats.maxTowerHealth;
        }
    }

    void DammageTower(int dammage)
    {
        towerHealth -= dammage;
        if (towerHealth <= 0 && !isDead)
        {
            isDead = true;
            player.GetComponent<PlayerHandler>().Kill();
            gameObject.SetActive(false);

        }
    }

}
