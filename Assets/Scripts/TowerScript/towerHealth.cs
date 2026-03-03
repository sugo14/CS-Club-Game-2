using UnityEngine;

public class TowerHealth : MonoBehaviour
{
    public int towerHealth;
    public bool isDead;
    public GameObject player;

    void Start()
    {
        isDead = false;
        gameObject.SetActive(true);
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
