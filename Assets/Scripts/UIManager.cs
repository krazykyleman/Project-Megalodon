using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Main UI")]
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI coinsPerSecondText;
    [SerializeField] private Button tapButton;
    [SerializeField] private TextMeshProUGUI tapButtonText;

    [Header("Upgrades")]
    [SerializeField] private Transform upgradeContainer;
    [SerializeField] private GameObject upgradeItemPrefab;

    [Header("Ads")]
    [SerializeField] private Button watchAdButton;
    [SerializeField] private TextMeshProUGUI adButtonText;

    [Header("Animation")]
    [SerializeField] private float tapAnimationScale = 1.2f;
    [SerializeField] private float tapAnimationDuration = 0.1f;

    private Vector3 originalTapButtonScale;

    private void Start()
    {
        if (tapButton != null)
        {
            originalTapButtonScale = tapButton.transform.localScale;
            tapButton.onClick.AddListener(OnTapButtonClicked);
        }

        if (watchAdButton != null)
        {
            watchAdButton.onClick.AddListener(OnWatchAdClicked);
            adButtonText.text = $"Watch Ad\n+{AdManager.Instance.rewardedAdCoins} Coins! 📺";
        }

        GameManager.Instance.OnCoinsChanged += UpdateUI;
        GameManager.Instance.OnUpgradeChanged += UpdateUI;

        CreateUpgradeButtons();
        UpdateUI();
    }

    private void CreateUpgradeButtons()
    {
        if (upgradeContainer == null || upgradeItemPrefab == null)
        {
            Debug.LogWarning("Upgrade UI not set up");
            return;
        }

        foreach (var upgrade in GameManager.Instance.upgrades)
        {
            GameObject item = Instantiate(upgradeItemPrefab, upgradeContainer);
            UpgradeButton upgradeButton = item.GetComponent<UpgradeButton>();

            if (upgradeButton != null)
            {
                upgradeButton.Setup(upgrade);
            }
        }
    }

    private void OnTapButtonClicked()
    {
        GameManager.Instance.OnTap();
        AdManager.Instance.OnTapOccurred();
        AnimateTapButton();
    }

    private void AnimateTapButton()
    {
        if (tapButton != null)
        {
            StopAllCoroutines();
            StartCoroutine(TapButtonAnimationCoroutine());
        }
    }

    private System.Collections.IEnumerator TapButtonAnimationCoroutine()
    {
        float elapsed = 0f;
        Vector3 targetScale = originalTapButtonScale * tapAnimationScale;

        // Scale up
        while (elapsed < tapAnimationDuration / 2)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / (tapAnimationDuration / 2);
            tapButton.transform.localScale = Vector3.Lerp(originalTapButtonScale, targetScale, t);
            yield return null;
        }

        elapsed = 0f;

        // Scale down
        while (elapsed < tapAnimationDuration / 2)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / (tapAnimationDuration / 2);
            tapButton.transform.localScale = Vector3.Lerp(targetScale, originalTapButtonScale, t);
            yield return null;
        }

        tapButton.transform.localScale = originalTapButtonScale;
    }

    private void OnWatchAdClicked()
    {
        watchAdButton.interactable = false;
        AdManager.Instance.ShowRewardedAd((success) =>
        {
            watchAdButton.interactable = true;
            if (success)
            {
                Debug.Log("Reward claimed successfully");
            }
        });
    }

    private void UpdateUI()
    {
        if (coinsText != null)
        {
            coinsText.text = $"💰 {FormatNumber(GameManager.Instance.coins)}";
        }

        if (coinsPerSecondText != null)
        {
            coinsPerSecondText.text = $"{FormatNumber(GameManager.Instance.coinsPerSecond)}/sec";
        }

        if (tapButtonText != null)
        {
            tapButtonText.text = "💎\nTap!";
        }
    }

    private string FormatNumber(double number)
    {
        if (number >= 1_000_000_000_000)
            return $"{number / 1_000_000_000_000:F2}T";
        if (number >= 1_000_000_000)
            return $"{number / 1_000_000_000:F2}B";
        if (number >= 1_000_000)
            return $"{number / 1_000_000:F2}M";
        if (number >= 1_000)
            return $"{number / 1_000:F2}K";
        return $"{number:F0}";
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnCoinsChanged -= UpdateUI;
            GameManager.Instance.OnUpgradeChanged -= UpdateUI;
        }
    }
}
