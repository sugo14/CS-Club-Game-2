using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button cardButton;

    public Image backgroundImage;   // Background (level-based)
    public Image CoverImage;        // Cover image

    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI descriptionText;

    public Sprite[] coverSprites;       // index = upgrade.id
    public Sprite[] backgroundSprites;  // 0 = level 1, 1 = level 2, 2 = level 3

    private Graphic[] allGraphics;

    private Color darkColor;
    private Color brightColor;

    private void Awake()
    {
        // Grab EVERYTHING under this card
        allGraphics = GetComponentsInChildren<Graphic>(true);

        brightColor = Color.white;

        darkColor = brightColor * 0.6f;
        darkColor.a = 1f;

        // Start dark
        SetAllGraphicsColor(darkColor);
    }

    private void SetAllGraphicsColor(Color color)
    {
        foreach (var g in allGraphics)
            g.color = color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetAllGraphicsColor(brightColor);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetAllGraphicsColor(darkColor);
    }

    public void SetUpgrade(Upgrade upgrade)
    {
        // Text
        if (cardNameText != null)
            cardNameText.text = upgrade.name;

        if (descriptionText != null)
            descriptionText.text = upgrade.description;

        // Cover image (by upgrade ID)
        if (CoverImage != null)
        {
            if (upgrade.id < coverSprites.Length)
                CoverImage.sprite = coverSprites[upgrade.id];
            else
                CoverImage.sprite = null;
        }

        // Background image (by level)
        if (backgroundImage != null)
        {
            int index = upgrade.level - 1;
            if (index >= 0 && index < backgroundSprites.Length)
                backgroundImage.sprite = backgroundSprites[index];
            else
                backgroundImage.sprite = null;
        }

        // Force dark after reroll
        SetAllGraphicsColor(darkColor);
    }

    public void ClearCoverImage()
    {
        if (CoverImage != null)
            CoverImage.sprite = null;
    }
}
