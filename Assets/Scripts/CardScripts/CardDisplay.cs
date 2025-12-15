using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button cardButton;
    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI descriptionText;
    public CardData cardData;

    private Image buttonImage;
    private Color darkColor;
    private Color brightColor;

    private int lastVersion = -1; // Track version for changes

    private void Awake()
    {
        buttonImage = cardButton.GetComponent<Image>();

        Color original = buttonImage.color;

        darkColor = original * 0.6f;
        darkColor.a = original.a;

        brightColor = original * 1.2f;
        brightColor.r = Mathf.Min(brightColor.r, 1f);
        brightColor.g = Mathf.Min(brightColor.g, 1f);
        brightColor.b = Mathf.Min(brightColor.b, 1f);
        brightColor.a = original.a;

        buttonImage.color = darkColor; // Start dark

        UpdateCardUI();
    }

    private void Update()
    {
        // Auto-update if ScriptableObject version changed
        if (cardData != null && cardData.version != lastVersion)
        {
            UpdateCardUI();
        }
    }

    private void OnDestroy()
    {
        if (cardData != null)
            cardData.onChanged -= UpdateCardUI;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonImage.color = brightColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImage.color = darkColor;
    }

    // Public method to update UI text
// Public method to update UI text
    public void UpdateCardUI()
    {
        if (cardData == null) return;

        if (cardNameText != null)
            cardNameText.text = cardData.cardTitle;

        if (descriptionText != null)
            descriptionText.text = cardData.cardDescription;

        lastVersion = cardData.version;
    }
    public void SetUpgrade(Upgrade upgrade)
    {
        cardNameText.text = upgrade.name;
        descriptionText.text = upgrade.description;
    }


#if UNITY_EDITOR
    private void OnValidate()
    {
        UpdateCardUI();
    }
#endif

}
