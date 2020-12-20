using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using GoogleMobileAds.Api.Mediation.MoPub;


public class AdmobManager : MonoBehaviour 
{
	//for creating the instance of admob manager 
	public static AdmobManager _instance{ set; get; }


	public bool _canShowAds=true;
	public bool _showTestAds=false;

	private BannerView _banner;
	private InterstitialAd _interstitial;
	private RewardBasedVideoAd _rewardVideo;

	[System.Serializable]
	public class _admobInfo
	{
		public string name;
		public string appID;
		public string bannerID;
		public string interstitialID;
		public string rewardVideoID;
	}

	public List<_admobInfo> _infoList = new List<_admobInfo> ();
	private string _appID,_bannerID,_interstitialID,_rewardVideoID;


	public string _mopubAppID;

	void Awake()
	{
		if (_instance != null) {
			return;
		} else {
			_instance = this;
		}
	}

	// Use this for initialization
	void Start () 
	{
		if (_canShowAds) 
		{
			#if UNITY_ANDROID
			_appID=_infoList[0].appID;
			_bannerID=_infoList[0].bannerID;
			_interstitialID=_infoList[0].interstitialID;
			_rewardVideoID=_infoList[0].rewardVideoID;
			#elif UNITY_IOS||UNITY_IPHONE
			_appID=_infoList[1].appID;
			_bannerID=_infoList[1].bannerID;
			_interstitialID=_infoList[1].interstitialID;
			_rewardVideoID=_infoList[1].rewardVideoID;
			#endif

			//initialization
			MobileAds.Initialize (_appID);

			//mopub initialization
			MoPub.Initialize(_mopubAppID);

			//requesing the ads when the game starts
			RequestForBannerAds ();
			RequestForInterstitialAds ();
			RequestForRewardVideoAds ();
		}
	}

	void RequestForBannerAds()
	{
		if (this._banner != null) 
		{
			this._banner.Destroy ();
		}
		else
		{
			this._banner = new BannerView (_bannerID, AdSize.SmartBanner, AdPosition.Bottom);
			AdRequest _bannerRequest;
		
			if (_showTestAds) 
			{
				_bannerRequest = new AdRequest.Builder ().AddTestDevice (AdRequest.TestDeviceSimulator).AddTestDevice ("0123456789ABCDEF0123456789ABCDEF").Build ();
			} else {
				_bannerRequest = new AdRequest.Builder ().Build ();
			}

			_banner.LoadAd (_bannerRequest);

			// Register for ad events.
			this._banner.OnAdLoaded += HandleAdLoaded;
			this._banner.OnAdFailedToLoad += HandleAdFailedToLoad;
			this._banner.OnAdOpening += HandleAdOpened;
			this._banner.OnAdClosed += HandleAdClosed;
			this._banner.OnAdLeavingApplication += HandleAdLeftApplication;
			ShowBannerAds ();
		}

	}

	public void DestroyBannerAds()
	{
		this._banner.Destroy ();
	}




	void RequestForInterstitialAds()
	{
		if (this._interstitial != null) 
		{
			this._interstitial.Destroy ();
		}

		this._interstitial = new InterstitialAd (_interstitialID);
		AdRequest _interstitialRequest;
	
		if(_showTestAds)
		{
			_interstitialRequest=new AdRequest.Builder ().AddTestDevice (AdRequest.TestDeviceSimulator).AddTestDevice ("0123456789ABCDEF0123456789ABCDEF").Build ();
		}
		else
		{
			_interstitialRequest= new AdRequest.Builder ().Build ();
		}

		this._interstitial.LoadAd (_interstitialRequest);

		// Register for ad events.
		this._interstitial.OnAdLoaded += this.HandleInterstitialLoaded;
		this._interstitial.OnAdFailedToLoad += this.HandleInterstitialFailedToLoad;
		this._interstitial.OnAdOpening += this.HandleInterstitialOpened;
		this._interstitial.OnAdClosed += this.HandleInterstitialClosed;
		this._interstitial.OnAdLeavingApplication += this.HandleInterstitialLeftApplication;
	}

	void RequestForRewardVideoAds()
	{
		this._rewardVideo=RewardBasedVideoAd.Instance;

		AdRequest _rewardVideoRequest;
		if (_showTestAds) 
		{
			_rewardVideoRequest = new AdRequest.Builder ().AddTestDevice (AdRequest.TestDeviceSimulator).AddTestDevice ("0123456789ABCDEF0123456789ABCDEF").Build ();
		} 
		else 
		{
			_rewardVideoRequest = new AdRequest.Builder ().Build ();
		}
		_rewardVideo.LoadAd (_rewardVideoRequest,_rewardVideoID);
	
		// RewardBasedVideoAd is a singleton, so handlers should only be registered once.
		this._rewardVideo.OnAdLoaded += this.HandleRewardBasedVideoLoaded;
		this._rewardVideo.OnAdFailedToLoad += this.HandleRewardBasedVideoFailedToLoad;
		this._rewardVideo.OnAdOpening += this.HandleRewardBasedVideoOpened;
		this._rewardVideo.OnAdStarted += this.HandleRewardBasedVideoStarted;
		this._rewardVideo.OnAdRewarded += this.HandleRewardBasedVideoRewarded;
		this._rewardVideo.OnAdClosed += this.HandleRewardBasedVideoClosed;
		this._rewardVideo.OnAdLeavingApplication += this.HandleRewardBasedVideoLeftApplication;
	}


	public void ShowBannerAds()
	{
		this._banner.Show ();
	}

	public void ShowInterstitialAds()
	{
		if (this._interstitial.IsLoaded ())
		{
			this._interstitial.Show ();
		} else {
			Debug.Log ("Interstitial ads has not been loaded !!");
		}
	}

	public void ShowRewardVideoAds()
	{
		if (_rewardVideo.IsLoaded ())
		{
			_rewardVideo.Show ();
		}
		else 
		{
			Debug.Log ("Reward video has not been loaded !!");
			FirebaseHandler._instance.TrackingRewarded(PlayerPrefs.GetInt ("PlayerSavedFor"),"no_ads");
		}
	}

	#region Banner callback handlers
	public void HandleAdLoaded(object sender, EventArgs args)
	{
	}
	public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
	}
	public void HandleAdOpened(object sender, EventArgs args)
	{
	}
	public void HandleAdClosed(object sender, EventArgs args)
	{
	}
	public void HandleAdLeftApplication(object sender, EventArgs args)
	{
	}
	#endregion

	#region Interstitial callback handlers
	public void HandleInterstitialLoaded(object sender, EventArgs args)
	{
		
	}
	public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		
	}
	public void HandleInterstitialOpened(object sender, EventArgs args)
	{
	}
	public void HandleInterstitialClosed(object sender, EventArgs args)
	{
	}
	public void HandleInterstitialLeftApplication(object sender, EventArgs args)
	{
	}
	#endregion

	#region RewardBasedVideo callback handlers
	public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
	{
		
	}
	public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		
	}
	public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
	{
	}
	public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
	{
	}
	public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
	{
	}
	//for rewarding the player
	public void HandleRewardBasedVideoRewarded(object sender, Reward args)
	{
		//calling game controller for saving the player
		GameController._instance.PlayerSaved ();
	}
	public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
	{
	}
	#endregion
}