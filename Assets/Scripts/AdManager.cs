using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdManager : MonoBehaviour {

    public static AdManager Ins;
    private InterstitialAd interstitial;
    // Use this for initialization
    void Awake () {
        if (Ins == null)
        {
            Ins = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(this);
        MobileAds.Initialize("ca-app-pub-2898674802591440~1457367819");
        this.interstitial = new InterstitialAd("ca-app-pub-2898674802591440/9770385376");
        AdRequest();
    }
    void AdRequest()
    {
        AdRequest request = new AdRequest.Builder().Build();
        this.interstitial.LoadAd(request);
    }
    public void ShowInterstitial()
    {
        if (this.interstitial.IsLoaded())
        {
            this.interstitial.Show();
            AdRequest();
        }
    }

}
