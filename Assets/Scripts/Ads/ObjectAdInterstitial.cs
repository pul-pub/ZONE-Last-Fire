using UnityEngine;
using YandexMobileAds.Base;
using YandexMobileAds;
using System;
using Firebase.Analytics;

public class ObjectAdInterstitial : MonoBehaviour
{
    [SerializeField] private string AdId = "R-M-9265562-1";
    private InterstitialAdLoader interstitialAdLoader;
    private Interstitial interstitial;

    private void Awake()
    {
        SetupLoader();
        RequestInterstitial();
        Debug.Log("Inicilasation Yandex Ads");
        DontDestroyOnLoad(gameObject);
    }

    private void SetupLoader()
    {
        interstitialAdLoader = new InterstitialAdLoader();
        interstitialAdLoader.OnAdLoaded += HandleInterstitialLoaded;
    }

    private void RequestInterstitial()
    {
        AdRequestConfiguration adRequestConfiguration = new AdRequestConfiguration.Builder(AdId).Build();
        interstitialAdLoader.LoadAd(adRequestConfiguration);
    }

    public void ShowInterstitial(PlayerInterface _pi)
    {
        if (interstitial != null)
        {
            interstitial.Show();
            interstitial.OnAdFailedToShow += _pi.Loader;
            interstitial.OnAdDismissed += _pi.Loader;
            _pi.Pause();
        }
    }

    public void HandleInterstitialLoaded(object sender, InterstitialAdLoadedEventArgs args)
    {
        // The ad was loaded successfully. Now you can handle it.
        interstitial = args.Interstitial;

        // Add events handlers for ad actions
        interstitial.OnAdClicked += OnClikInterstitial;
        interstitial.OnAdShown += OnStartShowInterstitial;
        interstitial.OnAdFailedToShow += OnFailInterstitial;
        interstitial.OnAdDismissed += OnEndShowInterstitial;
    }

    public void OnStartShowInterstitial(object sender, EventArgs args)
    {

    }

    public void OnEndShowInterstitial(object sender, EventArgs args)
    {
        if (StaticVal.firebaseApp != null)
            FirebaseAnalytics.LogEvent("viewing_ad_inter");
        DestroyInterstitial();
        RequestInterstitial();
    }

    public void OnFailInterstitial(object sender, EventArgs args)
    {
        DestroyInterstitial();
        RequestInterstitial();
    }

    public void OnClikInterstitial(object sender, EventArgs args)
    {

    }

    public void DestroyInterstitial()
    {
        if (interstitial != null)
        {
            interstitial.Destroy();
            interstitial = null;
        }
    }
}
