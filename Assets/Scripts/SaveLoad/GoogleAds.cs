using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class GoogleAds : MonoBehaviour
{
    #if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/1033173712"; //이건 테스트용 빌드 전에 새로 넣을 것 
    #elif UNITY_IPHONE
    private string _adUnitId = "ca-app-pub-3940256099942544/4411468910";
    #else
    private string _adUnitId = "unused";
    #endif
    private InterstitialAd _interstitialAd; 

    [SerializeField]
    private float randomPos = 0.5f;

    private bool hasTriggered = false;


    // Start is called before the first frame update
    void Start()
    {
        MobileAds.Initialize((InitializationStatus initStatus) => {});
        hasTriggered = PlayerPrefs.GetInt("flagAd", 0) == 1;
    }

    /// <summary>
    /// Loads the interstitial ad.
    /// </summary>
    public void LoadInterstitialAd()
    {
        // Clean up the old ad before loading a new one.
        if (_interstitialAd != null)
        {
                _interstitialAd.Destroy();
                _interstitialAd = null;
        }

        //Debug.Log("Loading the interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        InterstitialAd.Load(_adUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    //Debug.LogError("interstitial ad failed to load an ad " +"with error : " + error);
                    return;
                }

               // Debug.Log("Interstitial ad loaded with response : "+ ad.GetResponseInfo());

                _interstitialAd = ad;
            });
    }
    
    public void ShowInterstitialAd()
    {
        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
           // Debug.Log("Showing interstitial ad.");
            _interstitialAd.Show();
        }
        else
        {
           // Debug.LogError("Interstitial ad is not ready yet.");
        }
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    private void OnTriggerEnter2D(Collider2D col)
    {
        // 50%의 확률로 광고를 보여줍니다.
        if (Random.Range(0f, 1f) <= randomPos && !hasTriggered)
        {
            LoadInterstitialAd();
            Invoke("ShowInterstitialAd", 0.2f); // 0.2초 후에 ShowInterstitialAd 메서드를 호출합니다.
            hasTriggered = true;
             PlayerPrefs.SetInt("flagAd", hasTriggered ? 1 : 0);
        }
        
    }
    private void OnDestroy()
    {
        // 스크립트가 파괴될 때 PlayerPrefs 값을 저장합니다.
        PlayerPrefs.Save();
    }
}
