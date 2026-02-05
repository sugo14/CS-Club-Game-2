using UnityEngine;

public class towerHealth : MonoBehaviour
{
    public int TOWERHEALTH;
    public bool IS_DEAD; 
    public GameObject TOWER;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IS_DEAD = false;
        TOWER.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
        if(TOWERHEALTH == 0 && !IS_DEAD)
        {
            IS_DEAD = true;
            TOWER.SetActive(false);
        }
    }
}
