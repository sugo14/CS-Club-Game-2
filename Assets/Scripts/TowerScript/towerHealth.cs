using UnityEngine;

public class TowerHealth : MonoBehaviour
{
    public float towerHealth;
    public bool isDead;
    public GameObject player;

    public GameObject healthBarPrefab;
    private GameObject healthBar;


    [System.NonSerialized] public int playerID;
    [System.NonSerialized] public PlayerState playerState;

    void Start()
    {
        isDead = false;
        gameObject.SetActive(true);


        // Initialize tower health from player state
        if (playerState.id != 0 || playerID == 0)
        {
            towerHealth = playerState.roundStats.maxTowerHealth;
        }

        // Add health bar
        healthBar = Instantiate(healthBarPrefab);
        
        healthBar.transform.SetParent(transform, false);
        healthBar.transform.localPosition = new Vector3(0, -1.2f, 0);
        healthBar.transform.localScale = new Vector3(2, .2f, 1); 
    }

    public void DammageTower(float dammage)
    {
        towerHealth -= dammage;
        healthBar.GetComponent<HealhBar>().setFill(towerHealth / playerState.roundStats.maxTowerHealth);
        if (towerHealth <= 0 && !isDead)
        {
            isDead = true;
            player.GetComponent<PlayerHandler>().Kill();
            gameObject.SetActive(false);
            healthBar.SetActive(false);

        }
    }

}
