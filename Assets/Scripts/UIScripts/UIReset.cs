using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIReset : MonoBehaviour
{
    public GameObject Card1;
    public GameObject Card2;
    public GameObject Card3;
    public GameObject Card4;
    public GameObject cardSelectMenu;

    void Update()
    {
        if (Card1.activeSelf == false && Card2.activeSelf == false && Card3.activeSelf == false && Card4.activeSelf == false) 
        {
            cardSelectMenu.SetActive(false);
            Card1.SetActive(true);
            Card2.SetActive(true);
            Card3.SetActive(true);
            Card4.SetActive(true);
        }
    }
}
