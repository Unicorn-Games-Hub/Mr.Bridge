using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DayCounter : MonoBehaviour
{
	//day counter counts the day at the start of the game
	private ulong _timeCounter;

	// Use this for initialization
	void Start () 
	{
		if(PlayerPrefs.GetInt("FirstDayOfInstall")==0)
		{
			//setting up the timer
			_timeCounter = (ulong)DateTime.Now.Ticks;
			PlayerPrefs.SetString("TimeCounter",_timeCounter.ToString());
			//storing install day as day 1
			PlayerPrefs.SetInt("NumberOfDays", 1);
			//setting value so that it never be first day of install
			PlayerPrefs.SetInt("FirstDayOfInstall",1);  
		}
		FindOutTheDayOfInstallation ();
		//
		FirebaseHandler._instance.TrackingInstallDay (PlayerPrefs.GetInt ("NumberOfDays"));
	}

	void FindOutTheDayOfInstallation()
	{
		_timeCounter = ulong.Parse (PlayerPrefs.GetString ("TimeCounter"));

		ulong diff = ((ulong)DateTime.Now.Ticks - _timeCounter);
		ulong m = diff / TimeSpan.TicksPerMillisecond;
		float secondsLeft = (float)(86400000.0f - m) / 1000.0f;

		if (secondsLeft < 0) {
			CountTheDay ();
		} 
	}
		
	void CountTheDay()
	{
		PlayerPrefs.SetInt("NumberOfDays", PlayerPrefs.GetInt("NumberOfDays")+1);
		//setting up the timer
		_timeCounter = (ulong)DateTime.Now.Ticks;
		PlayerPrefs.SetString("TimeCounter",_timeCounter.ToString());
	}
}
