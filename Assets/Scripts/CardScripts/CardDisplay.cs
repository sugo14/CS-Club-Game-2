using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button cardButton;

    public Image backgroundImage;   // Background (level-based)
    public GameObject coverImageObject;       
    Image coverImage;               // Cover image

    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI descriptionText;

    public Sprite[] coverSprites;       // index = upgrade.id
    public Sprite[] backgroundSprites;  // 0 = level 1, 1 = level 2, 2 = level 3

    private Graphic[] allGraphics;

    private Color darkColor;
    private Color brightColor;

    private Upgrade currentUpgrade;          // stores this card's upgrade
    private UpgradeCardSpawner spawner;      // reference to spawner

    private void Awake()
    {
        allGraphics = GetComponentsInChildren<Graphic>(true);

        brightColor = Color.white;
        darkColor = brightColor * 0.6f;
        darkColor.a = 1f;

        SetAllGraphicsColor(darkColor);

    }

    private void SetAllGraphicsColor(Color color)
    {
        foreach (var g in allGraphics)
        {
            g.color = color;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetAllGraphicsColor(brightColor);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetAllGraphicsColor(darkColor);
    }

    public void SetSpawner(UpgradeCardSpawner spawnerRef)
    {
        spawner = spawnerRef;
    }

    public void SetUpgrade(Upgrade upgrade)
    {
        currentUpgrade = upgrade;

        if (cardNameText)
        {
            cardNameText.text = upgrade.name;
        }

        if (descriptionText)
        {
            descriptionText.text = upgrade.description;
        }

        // COVER IMAGE
        if (!coverImage.sprite)
        {
            if (upgrade.id >= 0 && upgrade.id < coverSprites.Length)
            {
                coverImage.sprite = coverSprites[upgrade.id];
            
            }
            else
            {
                coverImage.sprite = null;
            }
        }

        // BACKGROUND IMAGE (by level)
        if (backgroundImage)
        {
            int index = upgrade.level - 1;
            if (index >= 0 && index < backgroundSprites.Length)
            {
                backgroundImage.sprite = backgroundSprites[index];
            }
            else
            {
                backgroundImage.sprite = null;
            }
        }

        SetAllGraphicsColor(darkColor);
    }

    public void OnCardClicked()
    {
        if (spawner)
        {
            spawner.RegisterChoice(currentUpgrade);
            Debug.Log("Clicked: " + currentUpgrade.name); //confirm click
        }
        else
        {
            Debug.LogWarning("Spawner not set on card");
        }
        gameObject.SetActive(false);
    }

    public void ClearCoverImage()
    {
        // Assigning coverImage
        coverImage = coverImageObject.GetComponent<Image>();

        if (coverImage.sprite)
        {
            coverImage.sprite = null;
        }
    }
}