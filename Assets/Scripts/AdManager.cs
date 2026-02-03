using System;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static AdManager Instance { get; private set; }

    [Header("Ad Unit IDs")]
    [SerializeField] private string androidGameId = "your_android_game_id";
    [SerializeField] private string iosGameId = "your_ios_game_id";
    [SerializeField] private bool testMode = true;

    [Header("Ad Units")]
    [SerializeField] private string bannerAdUnitId = "Banner_Android";
    [SerializeField] private string interstitialAdUnitId = "Interstitial_Android";
    [SerializeField] private string rewardedAdUnitId = "Rewarded_Android";

    private string gameId;
    private Action<bool> onRewardedAdComplete;
    private int tapsUntilInterstitial = 50;
    private int currentTapCount = 0;

    [Header("Reward Settings")]
    public double rewardedAdCoins = 500;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAds();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeAds()
    {
#if UNITY_IOS
        gameId = iosGameId;
        bannerAdUnitId = "Banner_iOS";
        interstitialAdUnitId = "Interstitial_iOS";
        rewardedAdUnitId = "Rewarded_iOS";
#else
        gameId = androidGameId;
#endif

        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(gameId, testMode, this);
        }
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        LoadBannerAd();
        LoadInterstitialAd();
        LoadRewardedAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"Unity Ads Initialization Failed: {error} - {message}");
    }

    // Banner Ad
    public void LoadBannerAd()
    {
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };

        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Load(bannerAdUnitId, options);
    }

    private void OnBannerLoaded()
    {
        Debug.Log("Banner loaded");
        ShowBannerAd();
    }

    private void OnBannerError(string message)
    {
        Debug.LogError($"Banner Error: {message}");
    }

    public void ShowBannerAd()
    {
        BannerOptions options = new BannerOptions
        {
            showCallback = () => Debug.Log("Banner shown"),
            hideCallback = () => Debug.Log("Banner hidden"),
            clickCallback = () => Debug.Log("Banner clicked")
        };

        Advertisement.Banner.Show(bannerAdUnitId, options);
    }

    public void HideBannerAd()
    {
        Advertisement.Banner.Hide();
    }

    // Interstitial Ad
    public void LoadInterstitialAd()
    {
        Advertisement.Load(interstitialAdUnitId, this);
    }

    public void ShowInterstitialAd()
    {
        if (Advertisement.isInitialized)
        {
            Advertisement.Show(interstitialAdUnitId, this);
        }
        else
        {
            Debug.LogWarning("Interstitial ad not ready");
        }
    }

    public void OnTapOccurred()
    {
        currentTapCount++;
        if (currentTapCount >= tapsUntilInterstitial)
        {
            currentTapCount = 0;
            ShowInterstitialAd();
        }
    }

    // Rewarded Ad
    public void LoadRewardedAd()
    {
        Advertisement.Load(rewardedAdUnitId, this);
    }

    public void ShowRewardedAd(Action<bool> onComplete)
    {
        onRewardedAdComplete = onComplete;

        if (Advertisement.isInitialized)
        {
            Advertisement.Show(rewardedAdUnitId, this);
        }
        else
        {
            Debug.LogWarning("Rewarded ad not ready");
            onRewardedAdComplete?.Invoke(false);
        }
    }

    public bool IsRewardedAdReady()
    {
        return Advertisement.isInitialized;
    }

    // IUnityAdsLoadListener implementation
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log($"Ad Loaded: {adUnitId}");
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.LogError($"Ad Failed to Load: {adUnitId} - {error} - {message}");
    }

    // IUnityAdsShowListener implementation
    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"Ad Show Failed: {adUnitId} - {error} - {message}");

        if (adUnitId == rewardedAdUnitId)
        {
            onRewardedAdComplete?.Invoke(false);
        }

        // Reload the ad
        if (adUnitId == interstitialAdUnitId)
        {
            LoadInterstitialAd();
        }
        else if (adUnitId == rewardedAdUnitId)
        {
            LoadRewardedAd();
        }
    }

    public void OnUnityAdsShowStart(string adUnitId)
    {
        Debug.Log($"Ad Show Start: {adUnitId}");
        Time.timeScale = 0f; // Pause game during ad
    }

    public void OnUnityAdsShowClick(string adUnitId)
    {
        Debug.Log($"Ad Clicked: {adUnitId}");
    }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log($"Ad Show Complete: {adUnitId} - {showCompletionState}");
        Time.timeScale = 1f; // Resume game after ad

        if (adUnitId == rewardedAdUnitId)
        {
            if (showCompletionState == UnityAdsShowCompletionState.COMPLETED)
            {
                Debug.Log("Rewarded ad completed. Granting reward.");
                GameManager.Instance.AddRewardCoins(rewardedAdCoins);
                onRewardedAdComplete?.Invoke(true);
            }
            else
            {
                Debug.Log("Rewarded ad not completed.");
                onRewardedAdComplete?.Invoke(false);
            }
            LoadRewardedAd();
        }
        else if (adUnitId == interstitialAdUnitId)
        {
            LoadInterstitialAd();
        }
    }
}
