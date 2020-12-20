using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Analytics;
public class FirebaseHandler : MonoBehaviour 
{
	public static FirebaseHandler _instance{ set; get; }


	void Awake()
	{
		if (_instance != null) {
			return;
		} else {
			_instance = this;
		}
	}

	void Start()
	{
		FirebaseAnalytics.SetAnalyticsCollectionEnabled (true);
		FirebaseAnalytics.SetUserProperty (FirebaseAnalytics.UserPropertySignUpMethod, "google");
		FirebaseAnalytics.SetUserId("Karuwa_user_001");

		TrackAppOpenForTheFirstTime ();
	}

	//called when game is opened for the first time
	void TrackAppOpenForTheFirstTime()
	{
		if (PlayerPrefs.GetInt ("FirstTime") == 0)
		{
			Firebase.Analytics.FirebaseAnalytics.LogEvent ("app_open");
			Firebase.Analytics.FirebaseAnalytics.LogEvent("users","new_user","");
			PlayerPrefs.SetInt ("FirstTime", 1);
		}
	}

	//
	public void TrackingGameSession(int _sessionCount)
	{
		Firebase.Analytics.FirebaseAnalytics.LogEvent("session_count","count",_sessionCount);
	}

	//called when game level is loaded
	public void TrackingGamePlay(int _noOfgamesPlayed)
	{
		Firebase.Analytics.FirebaseAnalytics.LogEvent("gamePlay","gplay_count",_noOfgamesPlayed);
	}

	//counting the day of installation
	public void TrackingInstallDay(int _days)
	{
		Firebase.Analytics.FirebaseAnalytics.LogEvent("install_day","day_count",_days);
	}


	void TrackingInternetConnection()
	{
		
		if (Application.internetReachability == NetworkReachability.NotReachable) {
			Firebase.Analytics.FirebaseAnalytics.LogEvent("internet","action","off");
		} 
		else
		{
			Firebase.Analytics.FirebaseAnalytics.LogEvent("internet","action","on");
		}
	}

	void TrackingGDPR()
	{
		Firebase.Analytics.FirebaseAnalytics.LogEvent ("gdprpopup",new Parameter[]{
			new Firebase.Analytics.Parameter("shown","shown"),
			new Firebase.Analytics.Parameter("shown","gdpr_pp"),
			new Firebase.Analytics.Parameter("shown","locked"),
			new Firebase.Analytics.Parameter("action","accept")
		});
	}

	//called when level is loaded
	public void TrackingScreenStart()
	{
		Firebase.Analytics.FirebaseAnalytics.LogEvent ("screen", "level_start_screen", "");
	}

	//called when game is over
	public void TrackingScreenEnd()
	{
		Firebase.Analytics.FirebaseAnalytics.LogEvent ("screen", "level_end_screen", "");
	}

	//called when player click on rate button
	public void TrackingRating()
	{
		Firebase.Analytics.FirebaseAnalytics.LogEvent ("rate", "action", "tap");
	}

	//
	public void TrackingCharacter(int _charID,string _charStatus,int _requiredGems)
	{
		Firebase.Analytics.FirebaseAnalytics.LogEvent ("select_character",new Parameter[]{
			new Firebase.Analytics.Parameter("character_no",_charID),
			new Firebase.Analytics.Parameter("status",_charStatus),
			new Firebase.Analytics.Parameter("gems",_requiredGems)
		});
	}

	//
	public void TrackingGameStart(int _gemCount,string _startedfrom)
	{
		Firebase.Analytics.FirebaseAnalytics.LogEvent ("start_level",new Parameter[]{
			new Firebase.Analytics.Parameter("opening_gem_count",_gemCount),
			new Firebase.Analytics.Parameter("source",_startedfrom)
		});
	}

	//
	public void TrackingGameOver(int _gemCount,int _score,int _highScore)
	{
		Firebase.Analytics.FirebaseAnalytics.LogEvent ("end_level",new Parameter[]{
			new Firebase.Analytics.Parameter("closing_gem_count",_gemCount),
			new Firebase.Analytics.Parameter("score",_score),
			new Firebase.Analytics.Parameter("best_score",_highScore)
		});
	}

	//
	public void TrackingRewarded(int _videoAdsCounter,string _connection)
	{
		Firebase.Analytics.FirebaseAnalytics.LogEvent ("rewarded",new Parameter[]{
			new Firebase.Analytics.Parameter("save_me","tap"),
			new Firebase.Analytics.Parameter("video_count",_videoAdsCounter),
			new Firebase.Analytics.Parameter("popup",_connection)
		});
	}
}
