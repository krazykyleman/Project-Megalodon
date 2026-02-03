using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeButton : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI ownedText;
    [SerializeField] private TextMeshProUGUI productionText;
    [SerializeField] private Button buyButton;
    [SerializeField] private Image backgroundImage;

    private Upgrade upgrade;

    [Header("Colors")]
    [SerializeField] private Color affordableColor = new Color(0.2f, 0.8f, 0.2f, 1f);
    [SerializeField] private Color unaffordableColor = new Color(0.5f, 0.5f, 0.5f, 1f);

    private void Start()
    {
        if (buyButton != null)
        {
            buyButton.onClick.AddListener(OnBuyClicked);
        }

        GameManager.Instance.OnCoinsChanged += UpdateDisplay;
        GameManager.Instance.OnUpgradeChanged += UpdateDisplay;
    }

    public void Setup(Upgrade upgradeData)
    {
        upgrade = upgradeData;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (upgrade == null) return;

        bool canAfford = GameManager.Instance.coins >= upgrade.GetCurrentCost();

        if (nameText != null)
        {
            nameText.text = $"{upgrade.icon} {upgrade.upgradeName}";
        }

        if (descriptionText != null)
        {
            descriptionText.text = upgrade.description;
        }

        if (costText != null)
        {
            costText.text = $"Cost: {upgrade.GetDisplayCost()}";
            costText.color = canAfford ? Color.yellow : Color.gray;
        }

        if (ownedText != null)
        {
            ownedText.text = $"Owned: {upgrade.owned}";
        }

        if (productionText != null)
        {
            productionText.text = $"+{upgrade.GetDisplayProduction()}/sec";
        }

        if (buyButton != null)
        {
            buyButton.interactable = canAfford;
        }

        if (backgroundImage != null)
        {
            backgroundImage.color = canAfford ? affordableColor : unaffordableColor;
        }
    }

    private void OnBuyClicked()
    {
        if (upgrade != null)
        {
            GameManager.Instance.BuyUpgrade(upgrade);
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnCoinsChanged -= UpdateDisplay;
            GameManager.Instance.OnUpgradeChanged -= UpdateDisplay;
        }
    }
}
